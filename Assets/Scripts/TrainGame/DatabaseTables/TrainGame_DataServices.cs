﻿using SQLite4Unity3d;
using UnityEngine;
using System;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class TrainGame_DataServices  {

    private SQLiteConnection _connection;

    public TrainGame_DataServices(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
		// check if file exists in Application.persistentDataPath
		var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

		if (!File.Exists(filepath))
		{
		Debug.Log("Database not in Persistent path");
		// if it doesn't ->
		// open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
		var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
		while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
		// then save to Application.persistentDataPath
		File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
		var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#elif UNITY_WP8
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);

#endif

		Debug.Log("Database written");
		}

		var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //		_connection.CreateTable<Images>();
        Debug.Log("Final PATH: " + dbPath);

    }

    public IEnumerable<TrainGame_Levels> GetLevels()
    {
        return _connection.Table<TrainGame_Levels>();
    }
    //
    public IEnumerable<TrainGame_Levels> GetLevel(int id)
    {
        return _connection.Table<TrainGame_Levels>().Where(x => x.Id == id);
    }

    public IEnumerable<TrainGame_Levels> GetLevelsObject(int currentLevel)
    {
        return _connection.Table<TrainGame_Levels>().Where(x => x.LevelNumber == currentLevel);
    }

    public IEnumerable<TrainGame_Levels> GetRandomLevel()
    {
        const string command = "SELECT * FROM TrainGame_Levels ORDER BY RANDOM() LIMIT 1";
        return _connection.Query<TrainGame_Levels>(command);
    }

    public void UpdateUserProgress(string username, int level_number)
    {
        var user_level_obj = GetUserProgress(username);
        user_level_obj.Level_Obj = level_number;
        _connection.Update(user_level_obj);

    }

    public void MarkPreLevelCompleted(string username)
    {
        var user_level_obj = GetUserProgress(username);
        user_level_obj.PreLevelCompleted = BasketGame_SceneVariables.VALUE_FOR_PRE_LEVEL_COMPLETE;
        user_level_obj.LastModified = DateTime.Now;
        _connection.Update(user_level_obj);

    }

    public UserProgress_TrainGame GetUserProgress(string username)
    {
        //var _game_name = GetGameName();
        var user_level = _connection.Table<UserProgress_TrainGame>().Where(x => x.User_Obj == username);/*.Where(x => x.Game_name == _game_name);*/
        foreach (var user_level_obj in user_level)
        {
            return user_level_obj;
        }
        return AddUserProgress(username);
    }

    public UserProgress_TrainGame AddUserProgress(string username)
    {
        var default_level = 1;
        var max_ids = _connection.Query<UserProgress_TrainGame>("SELECT *, max(Id) FROM UserProgress_TrainGame LIMIT 1");
        int id = 0;
        foreach (var max_id in max_ids)
        {
            id = max_id.Id;
        }
        _connection.Insert(new UserProgress_TrainGame() { User_Obj = username, Level_Obj = default_level, DateCreated = DateTime.Now, LastModified = DateTime.Now });
        return GetUserProgress(username);
    }

    public void AddLevels(List<TrainGame_Levels> levels_list)
    {
        try
        {
            _connection.InsertAll(levels_list);
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        
    }

    public void AddLevel(TrainGame_Levels level_obj)
    {
        _connection.Insert(level_obj);
    }

    public void DeleteLevel(TrainGame_Levels level_obj)
    {
        _connection.Delete(level_obj);

    }

    public List<string> ListAllLevels()
    {
        var result = _connection.Table<TrainGame_Levels>();
        List<string> result_string = new List<string>();
        foreach (var r in result)
        {
            result_string.Add(r.ToString());
        }
        return result_string;
    }



}
