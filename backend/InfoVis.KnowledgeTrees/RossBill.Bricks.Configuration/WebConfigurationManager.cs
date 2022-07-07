namespace RossBill.Bricks.Configuration
{
    using Contracts;

    using System;
    using System.Linq;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.SqlClient;

    /// <summary>
    ///     ConfigurationManager that uses the web.config file to retrieve its settings
    ///     
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public class WebConfigConfigurationManager : IFileConfigurationManager
    {

        #region :: ConfigurationSettings ::

        public string GetConfigurationSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// Be aware that this only updates the current server, up ontill the next web cycle refresh. 
        /// Do not use this to set a property that should be changed on all the servers!
        /// </summary>
        public bool SetConfigurationSetting(string name, string value)
        {
            try
            {
                //Update setting if it exists, add new if it doesn't..
                if (ConfigurationManager.AppSettings.AllKeys.Contains(name))
                    ConfigurationManager.AppSettings[name] = value;
                else
                    ConfigurationManager.AppSettings.Add(name, value);

                return true;
            }
            catch (Exception /*ex*/)
            {
                //var something = ex.Message;
                //Something goes wrong, return false + log exception
                //Todo: Exception logging
                return false;
            }
        }

        #endregion

        #region :: Service Reference settings ::

        public string GetServiceReference(string serviceApplicationName)
        {
            return ConfigurationManager.AppSettings[serviceApplicationName];
        }

        public bool RegisterServiceReference(string serviceAppName, string serviceAddress)
        {
            //In webconfig, its just another setting...
            return SetConfigurationSetting(serviceAppName, serviceAddress);
        }

        #endregion

        #region :: DbConnection settings ::

        public DbConnection GetDbConnectionString(string connectionName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            return new EntityConnection(cs);
        }

        public bool SetDbConnectionString(string connectionName, string datasource, string initialCatalog)
        {
            //Get the connection
            var currentConnection = GetDbConnectionString(connectionName);

            //Update the initialcatalog & the datasource
            var builder = new SqlConnectionStringBuilder(currentConnection.ConnectionString)
            {
                DataSource = datasource,
                InitialCatalog = initialCatalog
            };

            //Update the connectionstring in the config file
            ConfigurationManager.ConnectionStrings[connectionName].ConnectionString = builder.ConnectionString;

            //Return result
            return true;
        }

        #endregion
    }
}