using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultimediaManager.Core;
using MultimediaManager.Core.FileSystem;
using MultimediaManager.Mp3;
using System.Collections.Generic;
using MultimediaManager.Core.Database;

namespace UnitTestProject
{
    [TestClass]
    public class BufferFileManagerTester
    {
        string file = "C:\\Temp\\test - Copy.mp3";

        [TestMethod]
        public void TestMethod1()
        {
            SQLiteTreeDatabase database = new SQLiteTreeDatabase();
            database.OpenOrCreate("E:\\mojabaza2.db");
            
            TreeViewDatabasEntity e = new TreeViewDatabasEntity()
            {
                Path = "Root",
                Name = "RootName",
                IsFile = false,
                Index = 0,
                ID = -1,
                ParentID = -1
            };
            database.Insert(e);
            var rootchilds = GenerateChilds(e, 4);
            foreach(var child in rootchilds)
            {
                database.InsertChild(child);
            }

            var listnodes = database.Select(SQLiteTreeDatabase.TREE_TABLE, null, null, null);

            long mid = database.Insert("MainTree");
            database.Insert(e.ID, mid);

          

            var listnodestee = database.GetRoots("MainTree");
  

        }

        IList<TreeViewDatabasEntity> GenerateChilds(TreeViewDatabasEntity e,int count)
        {
            List<TreeViewDatabasEntity> lsit = new List<TreeViewDatabasEntity>();
            for(int i=0; i<count ;i++)
            {
                lsit.Add(new TreeViewDatabasEntity()
                    {
                        ID = -1,
                        ParentID = e.ID,
                        Index = i,
                        IsFile = false,
                        Name = e.Name + "\\" + i,
                        Path = e.Path + "\\" + i
                    });
            }
            return lsit;
        }
    }
}
