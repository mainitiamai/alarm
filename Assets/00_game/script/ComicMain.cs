﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComicMain : PageBase
{
	public int m_iSelectingId;
	public ButtonManager m_bmIconList;

	private List<IconList> m_iconList = new List<IconList>();

	public ImageCheck m_imageCheck;

	public UIGrid m_Grid;

	public void IconSelect(int _iSelectIndex)
	{
		foreach (IconList icon in m_iconList)
		{
			icon.SetSelect(_iSelectIndex);
		}
		return;
	}

	// Use this for initialization
	void Start()
	{
		Debug.LogError ("here");
		Debug.LogError (DataManagerAlarm.Instance.m_csvConfig.Read ("footer"));
		if (DataManagerAlarm.Instance.m_csvConfig.Read ("footer").Contains ("Comic") == false) {
			gameObject.SetActive (false);
		}
		m_eStep = STEP.WAIT;
		m_eStepPre = STEP.MAX;
	}

	public enum STEP
	{
		NONE = 0,
		WAIT,
		IDLE,
		CHECKING,
		MAX,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public override void Initialize()
	{
		base.Initialize();
		if (m_eStep != STEP.WAIT)
		{
			m_eStep = STEP.IDLE;
		}
		m_eStepPre = STEP.MAX;
	}

	public override void Close()
	{
		base.Close();
		m_eStep = STEP.MAX;
	}

	// Update is called once per frame
	void Update()
	{

		bool bInit = false;
		if (m_eStepPre != m_eStep)
		{
			m_eStepPre = m_eStep;
			bInit = true;
		}

		switch (m_eStep)
		{
			case STEP.WAIT:

				if(SpriteManager.Instance.IsIdle())
				{

					m_bmIconList.ButtonRefresh(DataManagerAlarm.Instance.master_comic_list.Count);

					m_iSelectingId = GameMain.Instance.kvs_data.ReadInt(DataManagerAlarm.KEY_SELECTING_IMAGE_ID);

					int iIndex = 0;

					foreach (CsvImageData data in DataManagerAlarm.Instance.master_comic_list)
					{

						GameObject obj = PrefabManager.Instance.MakeObject("prefab/IconRoot", m_Grid.gameObject);
						IconList script = obj.GetComponent<IconList>();

						script.Initialize(m_iSelectingId, iIndex, data,m_Grid);

						m_iconList.Add(script);

						m_bmIconList.AddButtonBase(iIndex, obj);

						iIndex += 1;
					}

					IconSelect(m_iSelectingId);
					m_bmIconList.TriggerClearAll();

					m_imageCheck.Initialize();

					m_eStep = STEP.IDLE;
				}

				break;
			case STEP.IDLE:
				if (bInit)
				{
					m_bmIconList.TriggerClearAll();
				}
				if (m_bmIconList.ButtonPushed)
				{
					int iPushedId = m_iconList[m_bmIconList.Index].m_csvImageData.id;
					m_iSelectingId = iPushedId;
					m_bmIconList.TriggerClearAll();
					m_eStep = STEP.CHECKING;
				}
				break;

			case STEP.CHECKING:
				if (bInit)
				{
					m_imageCheck.TriggerClearAll();


					m_imageCheck.InStart(DataManagerAlarm.Instance.master_comic_list[m_iSelectingId].name_image);
				}
				if (m_imageCheck.ButtonPushed)
				{
					/*
					if (m_imageCheck.Index == 0)
					{
					}
					else if (m_imageCheck.Index == 1)
					{
						IconSelect(m_iSelectingId);
						GameMain.Instance.kvs_data.WriteInt(DataManagerAlarm.KEY_SELECTING_IMAGE_ID, m_iSelectingId);
						GameMain.Instance.kvs_data.Save();
					}
					else {
					}
					*/
					m_imageCheck.OutStart();
					m_eStep = STEP.IDLE;
				}
				break;
			case STEP.MAX:
			default:
				break;
		}


	}
}
