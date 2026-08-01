using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoreMountains.Tools;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;

public class AudioEditorWindow : BaseEditorWindow
{
    private static readonly BindingFlags BindingFlag =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private AudioManager _audioManager;
    private string _status = string.Empty;
    private Vector2 _scroll;

    private int _selectedChannelIndex;
    private int _selectedSongIndex;
    private int _selectedSfxIndex;
    private int _selectedUiIndex;
    private float _bgmVolume = 1f;
    private float _bgmPitch = 1f;

    private eSoundTracks _track = eSoundTracks.Music;
    private eSoundTrackEventTypes _trackEventType = eSoundTrackEventTypes.SetVolumeTrack;
    private float _trackVolume = 1f;

    private eAllSoundControlEventTypes _allTrackControlType = eAllSoundControlEventTypes.Pause;

    private eSoundTrackFadeEventModes _fadeMode = eSoundTrackFadeEventModes.PlayFade;
    private eSoundTracks _fadeTrack = eSoundTracks.Music;
    private float _fadeDuration = 1f;
    private float _fadeFinalVolume = 1f;
    private eFadeTrackTweenType _fadeTween = eFadeTrackTweenType.LinearTween;

    private readonly Dictionary<string, bool> _foldouts = new();

    [MenuItem("PhikozzLib/Audio Editor Window")]
    protected static void OpenWindow()
    {
        Open<AudioEditorWindow>("Audio Editor");
    }

    protected override void DrawGUI()
    {
        TitleLabel("Audio Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _audioManager = ObjectField(
            "AudioManager",
            _audioManager != null ? _audioManager : FindAnyObjectByType<AudioManager>(),
            true);

        if (_audioManager == null)
        {
            Error("AudioManager를 찾을 수 없습니다.");
            return;
        }

        DrawCurrentStatus();
        Space(8f);

        PlaylistDatabase playlistDatabase = null;
        SoundDatabase soundDatabase = null;

        if (TryGetPlaylistDatabase(out playlistDatabase))
        {
            DrawBgmControls(playlistDatabase);
            Space(8f);
        }

        if (TryGetSoundDatabase(out soundDatabase))
        {
            DrawSoundControls(soundDatabase);
            Space(8f);
        }

        DrawTrackControls();
        Space(8f);

        if (!string.IsNullOrEmpty(_status))
        {
            Info(_status);
            Space(8f);
        }

        DrawDatabaseLists(playlistDatabase, soundDatabase);
    }

    private void DrawCurrentStatus()
    {
        var playlistManager = MMSMPlaylistManager.Instance;

        BeginBox();
        BoldLabel("Current BGM Status");
        Label("State", playlistManager.DebugCurrentManagerState.ToString());
        Label("Playlist", playlistManager.Playlist != null ? playlistManager.Playlist.name : "-");
        Label("Song", string.IsNullOrEmpty(playlistManager.CurrentSongName) ? "-" : playlistManager.CurrentSongName);
        Label("Song Index", playlistManager.CurrentSongIndex.ToString());
        EndBox();
    }

    private void DrawBgmControls(PlaylistDatabase playlistDatabase)
    {
        var channels = playlistDatabase.PlaylistDic
            .OrderBy(x => x.Key)
            .ToArray();

        BeginBox();
        BoldLabel("BGM Test");
        Space();

        if (channels.Length == 0)
        {
            Label("등록된 Playlist 채널이 없습니다.");
            EndBox();
            return;
        }

        string[] channelOptions = channels
            .Select(x => $"{x.Key} : {x.Value.name}")
            .ToArray();

        _selectedChannelIndex = Mathf.Clamp(_selectedChannelIndex, 0, channelOptions.Length - 1);
        _selectedChannelIndex = Popup("Channel", _selectedChannelIndex, channelOptions);

        var selectedPlaylist = channels[_selectedChannelIndex].Value;
        var songs = selectedPlaylist != null && selectedPlaylist.Songs != null
            ? selectedPlaylist.Songs
            : new List<MMSMPlaylistSong>();

        string[] songOptions = songs.Count > 0
            ? songs.Select((song, index) =>
                $"{index} : {(!string.IsNullOrEmpty(song.Name) ? song.Name : song.Clip != null ? song.Clip.name : "Unnamed")}")
                .ToArray()
            : new[] { "0 : No Songs" };

        _selectedSongIndex = Mathf.Clamp(_selectedSongIndex, 0, Mathf.Max(0, songOptions.Length - 1));
        _selectedSongIndex = Popup("Song", _selectedSongIndex, songOptions);

        _bgmVolume = Slider("Volume", _bgmVolume, 0f, 1f);
        _bgmPitch = Slider("Pitch", _bgmPitch, 0f, 3f);

        BeginHorizontal();

        if (Button("Play BGM"))
        {
            _audioManager.PlayBgm(channels[_selectedChannelIndex].Key, _selectedSongIndex);
            _status = $"Play BGM: channel {channels[_selectedChannelIndex].Key}, song {_selectedSongIndex}";
        }

        if (Button("Stop BGM"))
        {
            _audioManager.StopBgm();
            _status = "Stop BGM";
        }

        if (Button("Pause BGM"))
        {
            _audioManager.PauseBgm();
            _status = "Pause BGM";
        }

        if (Button("Resume BGM"))
        {
            _audioManager.ResumeBgm();
            _status = "Resume BGM";
        }

        EndHorizontal();

        BeginHorizontal();

        if (Button("Prev"))
        {
            _audioManager.PlayPreviousBgm();
            _status = "Previous BGM";
        }

        if (Button("Next"))
        {
            _audioManager.PlayNextBgm();
            _status = "Next BGM";
        }

        if (Button("Apply Volume/Pitch"))
        {
            _audioManager.SetBgmMultiplier(_bgmVolume, _bgmPitch);
            _status = $"Set BGM multiplier: volume {_bgmVolume:0.00}, pitch {_bgmPitch:0.00}";
        }

        EndHorizontal();
        EndBox();
    }

    private void DrawSoundControls(SoundDatabase soundDatabase)
    {
        BeginBox();
        BoldLabel("SFX / UI Test");
        Space();

        var sfxKeys = soundDatabase.SfxSoundDataDic.Keys.OrderBy(x => x).ToArray();
        var uiKeys = soundDatabase.UiSoundDataDic.Keys.OrderBy(x => x).ToArray();

        if (sfxKeys.Length > 0)
        {
            _selectedSfxIndex = Mathf.Clamp(_selectedSfxIndex, 0, sfxKeys.Length - 1);
            _selectedSfxIndex = Popup("SFX", _selectedSfxIndex, sfxKeys);

            if (Button("Play SFX"))
            {
                _audioManager.PlaySfx(sfxKeys[_selectedSfxIndex]);
                _status = $"Play SFX: {sfxKeys[_selectedSfxIndex]}";
            }
        }
        else
        {
            Label("SFX", "등록된 SFX 키가 없습니다.");
        }

        Space();

        if (uiKeys.Length > 0)
        {
            _selectedUiIndex = Mathf.Clamp(_selectedUiIndex, 0, uiKeys.Length - 1);
            _selectedUiIndex = Popup("UI", _selectedUiIndex, uiKeys);

            if (Button("Play UI"))
            {
                _audioManager.PlayUi(uiKeys[_selectedUiIndex]);
                _status = $"Play UI: {uiKeys[_selectedUiIndex]}";
            }
        }
        else
        {
            Label("UI", "등록된 UI 키가 없습니다.");
        }

        EndBox();
    }

    private void DrawTrackControls()
    {
        BeginBox();
        BoldLabel("Track / Fade Test");
        Space();

        _track = EnumField("Track", _track);
        _trackEventType = EnumField("Track Event", _trackEventType);
        _trackVolume = Slider("Track Volume", _trackVolume, 0f, 1f);

        BeginHorizontal();

        if (Button("Apply Track Event"))
        {
            _audioManager.ControlTrack(_trackEventType, _track, _trackVolume);
            _status = $"Track Event: {_trackEventType} / {_track} / volume {_trackVolume:0.00}";
        }

        if (Button("Mute"))
        {
            _audioManager.ControlTrack(eSoundTrackEventTypes.MuteTrack, _track);
            _status = $"Mute Track: {_track}";
        }

        if (Button("Unmute"))
        {
            _audioManager.ControlTrack(eSoundTrackEventTypes.UnmuteTrack, _track);
            _status = $"Unmute Track: {_track}";
        }

        EndHorizontal();

        BeginHorizontal();

        if (Button("Play Track"))
        {
            _audioManager.ControlTrack(eSoundTrackEventTypes.PlayTrack, _track);
            _status = $"Play Track: {_track}";
        }

        if (Button("Pause Track"))
        {
            _audioManager.ControlTrack(eSoundTrackEventTypes.PauseTrack, _track);
            _status = $"Pause Track: {_track}";
        }

        if (Button("Stop Track"))
        {
            _audioManager.ControlTrack(eSoundTrackEventTypes.StopTrack, _track);
            _status = $"Stop Track: {_track}";
        }

        if (Button("Free Track"))
        {
            _audioManager.ControlTrack(eSoundTrackEventTypes.FreeTrack, _track);
            _status = $"Free Track: {_track}";
        }

        EndHorizontal();

        Space(8f);

        _allTrackControlType = EnumField("All Track Event", _allTrackControlType);

        if (Button("Apply All Track Event"))
        {
            _audioManager.ControlAllTrack(_allTrackControlType);
            _status = $"All Track Event: {_allTrackControlType}";
        }

        Space(8f);

        _fadeMode = EnumField("Fade Mode", _fadeMode);
        _fadeTrack = EnumField("Fade Track", _fadeTrack);
        _fadeDuration = FloatField("Fade Duration", _fadeDuration);
        _fadeFinalVolume = Slider("Fade Final Volume", _fadeFinalVolume, 0f, 1f);
        _fadeTween = EnumField("Fade Tween", _fadeTween);

        if (_fadeTrack == eSoundTracks.Master)
        {
            Warning("FadeTrack은 Master를 지원하지 않습니다.");
        }

        if (Button("Apply Fade"))
        {
            if (_fadeTrack == eSoundTracks.Master)
            {
                _status = "FadeTrack은 Master를 지원하지 않습니다.";
            }
            else
            {
                _audioManager.FadeTrack(
                    _fadeMode,
                    _fadeTrack,
                    Mathf.Max(0f, _fadeDuration),
                    _fadeFinalVolume,
                    _fadeTween);

                _status = $"Fade: {_fadeMode} / {_fadeTrack} / duration {Mathf.Max(0f, _fadeDuration):0.00} / volume {_fadeFinalVolume:0.00} / {_fadeTween}";
            }
        }

        EndBox();
    }

    private void DrawDatabaseLists(PlaylistDatabase playlistDatabase, SoundDatabase soundDatabase)
    {
        BoldLabel("Audio Lists");
        Space();

        _scroll = BeginScrollView(_scroll, GUILayout.Height(280f));

        if (playlistDatabase != null)
        {
            DrawPlaylistList(playlistDatabase);
            Space();
        }

        if (soundDatabase != null)
        {
            DrawSoundList("SFX Keys", "sfx", soundDatabase.SfxSoundDataDic.Keys.OrderBy(x => x));
            Space();
            DrawSoundList("UI Keys", "ui", soundDatabase.UiSoundDataDic.Keys.OrderBy(x => x));
        }

        EndScrollView();
    }

    private void DrawPlaylistList(PlaylistDatabase playlistDatabase)
    {
        SetFoldoutDefault("playlists", true);
        _foldouts["playlists"] = Foldout(_foldouts["playlists"], $"Playlists ({playlistDatabase.PlaylistDic.Count})");

        if (!_foldouts["playlists"])
            return;

        BeginIndent();

        foreach (var pair in playlistDatabase.PlaylistDic.OrderBy(x => x.Key))
        {
            string foldoutKey = $"playlist_{pair.Key}";
            SetFoldoutDefault(foldoutKey, false);

            int songCount = pair.Value != null && pair.Value.Songs != null ? pair.Value.Songs.Count : 0;
            _foldouts[foldoutKey] = Foldout(_foldouts[foldoutKey], $"{pair.Key} : {pair.Value.name} ({songCount})");

            if (!_foldouts[foldoutKey])
                continue;

            BeginIndent();

            if (pair.Value == null || pair.Value.Songs == null || pair.Value.Songs.Count == 0)
            {
                Label("- No Songs -");
            }
            else
            {
                for (int i = 0; i < pair.Value.Songs.Count; i++)
                {
                    var song = pair.Value.Songs[i];
                    string songName = !string.IsNullOrEmpty(song.Name)
                        ? song.Name
                        : song.Clip != null ? song.Clip.name : "Unnamed";

                    Label($"{i}. {songName}");
                }
            }

            EndIndent();
        }

        EndIndent();
    }

    private void DrawSoundList(string title, string foldoutKey, IEnumerable<string> keys)
    {
        string[] keyArray = keys.ToArray();

        SetFoldoutDefault(foldoutKey, true);
        _foldouts[foldoutKey] = Foldout(_foldouts[foldoutKey], $"{title} ({keyArray.Length})");

        if (!_foldouts[foldoutKey])
            return;

        BeginIndent();

        if (keyArray.Length == 0)
        {
            Label("- Empty -");
        }
        else
        {
            foreach (var key in keyArray)
            {
                Label(key);
            }
        }

        EndIndent();
    }

    private void SetFoldoutDefault(string key, bool value)
    {
        if (!_foldouts.ContainsKey(key))
            _foldouts[key] = value;
    }

    private bool TryGetPlaylistDatabase(out PlaylistDatabase database)
    {
        database = GetPrivateField<PlaylistDatabase>(_audioManager, "_playlistDatabase");
        return database != null;
    }

    private bool TryGetSoundDatabase(out SoundDatabase database)
    {
        database = GetPrivateField<SoundDatabase>(_audioManager, "_soundDatabase");
        return database != null;
    }

    private static T GetPrivateField<T>(object target, string fieldName) where T : class
    {
        if (target == null)
            return null;

        var field = target.GetType().GetField(fieldName, BindingFlag);
        return field?.GetValue(target) as T;
    }
}