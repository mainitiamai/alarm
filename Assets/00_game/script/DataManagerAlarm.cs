﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class DataManagerAlarm : DataManagerBase<DataManagerAlarm> {

	public readonly string KEY_CONFIG_VERSION = "config_version";
	public readonly string KEY_DOWNLOAD_VERSION = "download_version";
	public readonly string KEY_COMIC_LIST_VERSION = "comic_list_version";
	public readonly string KEY_IMAGE_LIST_VERSION = "image_list_version";

	public readonly string FILENAME_CONFIG = "config";
	public readonly string FILENAME_DOWNLOAD_LIST = "donwload_list";
	public readonly string FILENAME_COMIC_LIST = "comic_list";
	public readonly string FILENAME_IMAGE_LIST = "image_list";

	public CsvImage m_csvImage = new CsvImage();
	public List<CsvImageData> master_image_list {
		get{ 
			return Instance.m_csvImage.All;
		}
	}
	public CsvVoice m_csvVoice = new CsvVoice();
	public List<CsvVoiceData> master_voice_list {
		get{ 
			return Instance.m_csvVoice.All;
		}
	}
	public CsvVoiceset m_csvVoiceset = new CsvVoiceset();
	public List<CsvVoicesetData> master_voiceset_list
	{
		get
		{
			return Instance.m_csvVoiceset.All;
		}
	}

	public CsvImage m_csvComic = new CsvImage("csv/comic_list");
	public List<CsvImageData> master_comic_list
	{
		get
		{
			return Instance.m_csvComic.All;
		}
	}
	public CsvAudio m_masterTableAudio = new CsvAudio();
	public CsvPrefab m_masterTablePrefab = new CsvPrefab();
	public CsvSprite m_masterTableSprite = new CsvSprite();

	private CsvKvs m_Config = new CsvKvs();
	public CsvKvs config{
		get{ return m_Config; }
	}

	public override void Initialize ()
	{
		base.Initialize ();
		m_masterTableAudio.Load ("dummy");
		m_masterTablePrefab.Load ("dummy");
		m_masterTableSprite.Load ("dummy");
		SpriteManager.Instance.csv_sprite_list = m_masterTableSprite.All;
		PrefabManager.Instance.csv_prefab_list = m_masterTablePrefab.All;
		SoundManager.Instance.m_csvAudioDataList = m_masterTableAudio.All;

		m_csvImage.Load ();
		m_csvVoice.Load ();
		m_csvVoiceset.Load ();

		m_Config.Load (FILENAME_CONFIG);
		kvs.Load (FILENAME_KVS);
		if (config.Read ("footer").Contains ("Comic")) {
			m_csvComic.Load (FILENAME_COMIC_LIST);
		}
		foreach( CsvKvsParam param in config.All)
		{
			Debug.LogError( string.Format( "{0}:{1}", param.key,param.value));
		}
	}
	public string [] STR_MONTH_SHORT_ARR = new string[13]{
		"NONE",
		"JAN",
		"FEB",
		"MAR",
		"APR",
		"MAY",
		"JUN",
		"JUL",
		"AUG",
		"SEP",
		"OCT",
		"NOV",
		"DEC"
	};

	public string [] STR_WEEK_ARR = new string[7]{
		"Monday",
		"Tuesday",
		"Wednesday",
		"Thursday",
		"Friday",
		"Saturday",
		"Sunday"
	};
	public string [] STR_WEEK_SHORT_ARR = new string[7]{
		"Mon",
		"Tue",
		"Wed",
		"Thu",
		"Fri",
		"Sat",
		"Sun"
	};
	public string [] STR_SNOOZE_ARR = new string[3]{
		"5 min",
		"10 min",
		"None",
	};
	public const string KEY_SELECTING_IMAGE_ID = "selecting_image_id";

	public CsvVoiceData GetVoiceData( int _iId ){
		foreach (CsvVoiceData voice_data in DataManagerAlarm.Instance.master_voice_list) {
			if (_iId == voice_data.id) {
				return voice_data;
			}
		}
		return new CsvVoiceData ();
	}
	#if UNITY_ANDROID
	public List<GoogleSkuInfo> product_data_list = new List<GoogleSkuInfo> ();
	#endif
	public List<string> purchased_list = new List<string> ();
}

[System.Serializable]
public class AlarmReserve{
	public long m_lTime;
	public string m_strTime;
	public int m_iVoiceType;
	public int m_iSnoozeType;
}


