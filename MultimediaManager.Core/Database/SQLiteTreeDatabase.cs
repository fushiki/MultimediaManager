using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MultimediaManager.Core.Database
{
    public class SQLiteTreeDatabase:ITreeDatabase
    {
        #region Constans

        public const String TREE_TABLE = "Trees";
        public const String TREE_ID = "T_ID";
        public const String TREE_PARENT = "Parent";
        public const String TREE_INDEX = "Indexp";
        public const String TREE_PATH = "Path";
        public const String TREE_NAME = "Name";
        public const String TREE_IS_FILE = "IsFile";

        public const String INFO_ID = "I_ID";
        public const String INFO_TABLE_NAME = "TreeInfo";
        public const String INFO_NAME = "TreeName";

        public const String ROOTS_TABLE_NAME = "Roots";
        public const String ROOTS_INFO_FK = "InfoID";
        public const String ROOTS_TREE_FK = "TreeNodeId";
        public const String ROOTS_ID = "R_ID";

        public const String VIEW_ROOTS = "ViewRoots";

        #endregion

        #region Querrys
        public static readonly String CREATE_TREE_TABLE =
            (new StringBuilder()).Append
            (String.Format("CREATE TABLE {0}(", TREE_TABLE)).Append
            (String.Format("{0} integer primary key autoincrement,", TREE_ID)).Append
            (String.Format("{0} integer,", TREE_PARENT)).Append
            (String.Format("{0} integer,", TREE_INDEX)).Append
            (String.Format("{0} integer,", TREE_IS_FILE)).Append
            (String.Format("{0} text,", TREE_NAME)).Append
            (String.Format("{0} text);", TREE_PATH)).ToString();

        public static readonly String CREATE_TREE_INFO_TABLE =
            (new StringBuilder()).Append
            (String.Format("CREATE TABLE {0}(", INFO_TABLE_NAME)).Append
            (String.Format("{0} integer primary key autoincrement,", INFO_ID)).Append
            (String.Format("{0} text);", INFO_NAME)).ToString();

        public static readonly String CREATE_ROOTS_TABLE =
            (new StringBuilder()).Append
            (String.Format("CREATE TABLE {0}(", ROOTS_TABLE_NAME)).Append
            (String.Format("{0} integer primary key autoincrement,", ROOTS_ID)).Append
            (String.Format("{0} integer REFERENCES {1} ({2}) ON DELETE CASCADE,", ROOTS_INFO_FK, INFO_TABLE_NAME, INFO_ID)).Append
            (String.Format("{0} integer REFERENCES {1} ({2}) ON DELETE CASCADE);", ROOTS_TREE_FK, TREE_TABLE, TREE_ID)).ToString();

        /// <summary>
        /// Insert Parent,Index,Path,IsFile
        /// </summary>
        public static readonly String TREE_INSERT = String.Format(
            "INSERT INTO {0} ({1},{2},{3},{4},{5}) VALUES (@{1},@{2},@{3},@{4},@{5});",
            TREE_TABLE, TREE_PARENT, TREE_INDEX, TREE_IS_FILE,TREE_NAME, TREE_PATH);

        public static readonly String ROOTS_INSERT = String.Format(
            "INSERT INTO {0} ({1},{2}) VALUES (@{1},@{2});", ROOTS_TABLE_NAME, ROOTS_INFO_FK, ROOTS_TREE_FK);

        public static readonly String INFO_INSERT = String.Format(
            "INSERT INTO {0} ({1}) VALUES (@{1});", INFO_TABLE_NAME, INFO_NAME);

        public static readonly String SELECT_ROOTS = 
("SELECT "+TREE_TABLE+".*,INFO_ROOTS."+INFO_NAME +" FROM (" +
    "SELECT " + INFO_NAME +","+ROOTS_TREE_FK +" FROM " + ROOTS_TABLE_NAME + " JOIN " + INFO_TABLE_NAME + " ON "+ ROOTS_INFO_FK + 
    "="+INFO_ID+") AS INFO_ROOTS JOIN " + TREE_TABLE +" ON " + TREE_ID + "= INFO_ROOTS." + ROOTS_TREE_FK +";");

        public static readonly String CREATE_ROOTS_VIEW =
            String.Format("CREATE VIEW {0} AS {1}", VIEW_ROOTS, SELECT_ROOTS);

        /// <summary>
        /// Delete Id
        /// </summary>
        public static readonly String DELETE_FORMAT =
            "DELETE FROM {0} WHERE {1}=@{1};";

        /// <summary>
        /// Update Parent,Sibling,Path,Id
        /// </summary>
        public static readonly String UPDATE_TREE = string.Format(
            "UPDATE {0} SET {1}=@{1}, {2}=@{2}, {3}=@{3}, {4}=@{4}, {5}=@{5} WHERE {6}=@{6};",
            TREE_TABLE, TREE_PARENT, TREE_INDEX, TREE_PATH,TREE_NAME,TREE_IS_FILE, TREE_ID);

        public static readonly String SELECT_ALL = String.Format(
            "SELECT * FROM {0};", TREE_TABLE);

        public static readonly String SELECT_ALL_ORDER_PARENT_INDEX =
            String.Format("SELECT * FROM {0} ORDER BY {1},{2};", TREE_TABLE, TREE_PARENT, TREE_INDEX);

        public static readonly String SELECT_FORMAT ="SELECT {1} FROM {0} {2} {3};";
        public static readonly String SELECT_FORMAT_ALL = "*";
        public static readonly String SELECT_FORMAT_WHERE = "WHERE {0}";
        public static readonly String SELECT_FORMAT_ORDERBY = "ORDER BY {0}";

        public static readonly String UPDATE_FORMAT = "UPDATE {0} SET {1} WHERE {2};";

        #endregion

        #region Fields

        SQLiteConnection _connection;

        #endregion

        #region Properties

        public bool HasActiveConnection
        {
            get
            {
                return _connection != null &&
                    _connection.State != System.Data.ConnectionState.Broken &&
                    _connection.State != System.Data.ConnectionState.Closed;
            }
        }

        #endregion



      

        /// <summary>
        /// Open connection to database. Returns true if database was created during open.
        /// </summary>
        /// <param name="databasePath">Database file path.</param>
        /// <returns>Return true if database was created during open.</returns>
        /// <exception cref="SQLiteException"></exception>
        public bool Open(string databasePath)
        {
            if(HasActiveConnection)
                Dispose();
            bool exists = System.IO.File.Exists(databasePath);
            try
            {
                _connection = new SQLiteConnection(CreateConnectionString(databasePath));
                _connection.Open();                
            }catch(SQLiteException sqlitex)
            {
                _connection = null;
                throw sqlitex;
            }
            return !exists;
        }

        /// <summary>
        /// Open database and if it is new then create tables.
        /// </summary>
        /// <param name="databasePath">Database file path.</param>
        /// <exception cref="SQLiteException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void OpenOrCreate(string databasePath)
        {
            bool newdatabase = Open(databasePath);

            if(newdatabase)
            {
                try
                {
                    Create();
                }catch(SQLiteException sqlex)
                {
                    Dispose();
                    throw sqlex;
                }
            }
        }

        /// <summary>
        /// Create tables in database
        /// </summary>
        /// <exception cref="SQLiteException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Create()
        {
            if (!HasActiveConnection)
                throw new InvalidOperationException("Operation on not active database connection");
            using(SQLiteCommand cmd = _connection.CreateCommand())
            {
                using(SQLiteTransaction trn = _connection.BeginTransaction())
                {
                    try
                    {
                        cmd.CommandText = CREATE_TREE_TABLE;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = CREATE_TREE_INFO_TABLE;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = CREATE_ROOTS_TABLE;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = CREATE_ROOTS_VIEW;
                        cmd.ExecuteNonQuery();
                    }catch(SQLiteException sqlex)
                    {
                        trn.Rollback();
                        var ex = new SQLiteTreeException("Exception during create tables",sqlex);
                        Logger.Error(ex);
                        throw ex;
                    }
                    trn.Commit();
                }
            }
        }

        public void Update(string table,string how,String where)
        {
            if (where == null) where = String.Empty;
            if (how == null) throw new ArgumentNullException("How cannot be null");
            ExecuteQuery<int>
                ( x=>
                    {
                        x.CommandText = String.Format(UPDATE_FORMAT, table, how, where);
                        x.ExecuteNonQuery();
                        return -1;
                    },true,"Error during update.",new Dictionary<string,object>()
                    
                );
        }

        public long Insert(string tree)
        {
            return ExecuteQuery<long>(
                x =>
                {
                    x.CommandText = INFO_INSERT;
                    x.ExecuteNonQuery();
                    return _connection.LastInsertRowId;
                }, true, "Failed insert new tree.", new Dictionary<string, object>() { { INFO_NAME, tree } });
        }

        public long Insert(long rootid,string treename)
        {
            var reader = SelectWithReader(INFO_TABLE_NAME, null, INFO_NAME + "=" + treename, null);
            long id = -1;
            try
            {
                if (reader.Read())
                {
                    id = reader.GetInt64(reader.GetOrdinal(INFO_ID));
                }
                else
                {
                    return -1;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    reader.Dispose();
                }
                catch
                {
                    //
                }
            }
            if (id == -1) throw new SQLiteTreeException("Tree not found.");
            return Insert(rootid, id);
        }
        public long Insert(long rootid, long treeId)
        {
            return ExecuteQuery<long>
                (
                x=>{
                    x.CommandText = ROOTS_INSERT;
                    x.ExecuteNonQuery();
                    return _connection.LastInsertRowId;
                },true,"Failed to insert new root.",new Dictionary<string,object>()
                {{ROOTS_INFO_FK,treeId},
                {ROOTS_TREE_FK,rootid}});
        }
        public long Insert(TreeViewDatabasEntity entity)
        {           
       
            long value = ExecuteQuery<long>(ExecuteTreeInsert,true,"Exception during insert.",
                new Dictionary<string,object>(){
                    {TREE_PARENT,entity.ParentID},
                    {TREE_INDEX,entity.Index},
                    {TREE_PATH,entity.Path},
                    {TREE_IS_FILE,entity.IsFile? 1:0},
                    {TREE_NAME,entity.Name}
                });
            entity.ID = value;
            return value;
        }

        private long ExecuteTreeInsert(SQLiteCommand cmd)
        {
            cmd.CommandText = TREE_INSERT;
            cmd.ExecuteNonQuery();
            return _connection.LastInsertRowId;
        }

        public long Delete(long id)
        {
            return ExecuteQuery<long>(ExecuteDelete, true, "Exception during delete.",
                new Dictionary<string, object>(){
                    {TREE_ID,id}
                });
        }

        private long ExecuteDelete(SQLiteCommand command)
        {
            command.CommandText = DELETE_FORMAT;
            return command.ExecuteNonQuery();
        }


        public IList<TreeViewDatabasEntity> SelectAll(bool doubleorder)
        {
            return null;
        }
        public IList<long> GetAllIds()
        {
            return ExecuteQuery<IList<long>>(ExecuteGetAllIds,false,"Failed to get all id's.",
                new Dictionary<string,object>());
        }

        private IList<long> ExecuteGetAllIds(SQLiteCommand cmd)
        {
            cmd.CommandText = SELECT_ALL;
            List<long> ids = new List<long>();
 	        using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    ids.Add(reader.GetInt64(reader.GetOrdinal(TREE_ID)));
                }
            }
            return ids;
        }
        public void Dispose()
        {

        }
        public SQLiteDataReader SelectWithReader(string table, string[] columns, String where, String orderby)
        {
            String querry = PrepareSelect(table, columns, where, orderby);

            return ExecuteQuery<SQLiteDataReader>
                (cmd =>
                {
                    
                    cmd.CommandText = querry;
                    return cmd.ExecuteReader();
                }, false, "Failed execute Select statemant", new Dictionary<string, object>()
                );
        }
        public IList<TreeViewDatabasEntity> Select(string table,string[] columns, String where, String orderby)
        {
            String querry = PrepareSelect(table,columns, where, orderby);
           
            return ExecuteQuery<IList<TreeViewDatabasEntity>>
                (cmd =>
                    {
                        List<TreeViewDatabasEntity> list = new List<TreeViewDatabasEntity>();
                        cmd.CommandText = querry;
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TreeViewDatabasEntity entity = new TreeViewDatabasEntity();
                                if (columns == null || columns.Contains(TREE_ID))
                                    entity.ID = reader.GetInt64(reader.GetOrdinal(TREE_ID));
                                if (columns == null || columns.Contains(TREE_INDEX))
                                    entity.Index = reader.GetInt32(reader.GetOrdinal(TREE_INDEX));
                                if (columns == null || columns.Contains(TREE_IS_FILE))
                                    entity.IsFile = reader.GetInt32(reader.GetOrdinal(TREE_IS_FILE)) > 0;
                                if (columns == null || columns.Contains(TREE_NAME))
                                    entity.Name = reader.GetString(reader.GetOrdinal(TREE_NAME));
                                if (columns == null || columns.Contains(TREE_PARENT))
                                    entity.ParentID = reader.GetInt64(reader.GetOrdinal(TREE_PARENT));
                                if (columns == null || columns.Contains(TREE_PATH))
                                    entity.Path = reader.GetString(reader.GetOrdinal(TREE_PATH));
                                list.Add(entity);
                            }
                        }
                        return list;
                    },false,"Failed execute Select statemant",new Dictionary<string,object>()
                );
        }

        private string PrepareSelect(string table,string[] columns, string where, string orderby)
        {
            if (table == null) throw new ArgumentNullException("Table name.");
            String _columns, _where, _order;
            if (columns == null || columns.Length == 0)
            {
                _columns = SELECT_FORMAT_ALL;
            }
            else
            {
                _columns = String.Join(",", columns);
            }

            if (where == null || where.Count() == 0)
            {
                _where = String.Empty;
            }
            else
            {
                _where = String.Format(SELECT_FORMAT_WHERE, where);
            }

            if (orderby == null || orderby.Count() == 0)
            {
                _order = String.Empty;
            }
            else
            {
                _order = String.Format(SELECT_FORMAT_ORDERBY, orderby);
            }
            return String.Format(SELECT_FORMAT, table, _columns, _where, _order);
        }

        #region Private Methods

        private String CreateConnectionString(string dataSource)
        {
            return String.Format("Data Source={0}", dataSource);
        }



        private T ExecuteQuery<T>(Func<SQLiteCommand, T> execution, bool transaction, string failureMsg, Dictionary<string, object> parameters)
        {
            if (!HasActiveConnection)
                throw new InvalidOperationException("Operation on not active database connection");

            T result = default(T);
            using(SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                foreach(var pair in parameters)
                {
                    cmd.Parameters.AddWithValue("@"+pair.Key,pair.Value);
                }
                if(transaction)
                {
                    using(SQLiteTransaction trn = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = execution(cmd);
                        }catch(SQLiteException sqlex)
                        {
                            trn.Rollback();
                            var ex = new SQLiteTreeException(failureMsg, sqlex);
                            Logger.Error(ex);
                            throw ex;
                        }
                        trn.Commit();
                    }
                }else
                {
                    try
                    {
                        result = execution(cmd);
                    }
                    catch (SQLiteException sqlex)
                    {
                        var ex = new SQLiteTreeException(failureMsg, sqlex);
                        Logger.Error(ex);
                        throw ex;
                    }
                }
            }
            return result;
        }
        #endregion

        public IList<TreeViewDatabasEntity> GetChilds(long key)
        {
            return Select(TREE_TABLE,null, TREE_PARENT + "=" + key, null);
        }

        public long InsertChild(TreeViewDatabasEntity child)
        {
            if(child.ParentID!=-1)
                Update(TREE_TABLE,
                String.Format("{0} = {0}+1",TREE_INDEX),
                String.Format("{0} = {1} AND {2} >= {3}",TREE_PARENT,child.ParentID, TREE_INDEX,child.Index));
            return Insert(child);
        }

        public void RemoveChild(TreeViewDatabasEntity child)
        {
            Delete(child.ID);
            if(child.ParentID!=-1)
               Update(TREE_TABLE,
               String.Format("{0} = {0}-1", TREE_INDEX),
               String.Format("{0} = {1} AND {2} >= {3}", TREE_PARENT, child.ParentID, TREE_INDEX, child.Index));
        }

        public IList<TreeViewDatabasEntity> GetRoots(string name)
        {
            return Select(VIEW_ROOTS, null, INFO_NAME + "='" + name+"'", null);
        }
    }
}
