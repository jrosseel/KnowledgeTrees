namespace RossBill.Bricks.Configuration.Contracts
{
    using System.Data.Common;
    /// <summary>
    ///     Abstraction of a configuration manager
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        ///     Gets a setting from the configurationmanager
        /// </summary>
        /// <param name="name">The key of the setting</param>
        /// <returns>The setting</returns>
        string GetConfigurationSetting(string name);

        /// <summary>
        /// Sets a setting from the configurationmanager
        /// </summary>
        /// <param name="name">The key of the setting</param>
        /// <param name="value">The new value of the setting</param>
        /// <returns>True if it succeeded, false if it didn't</returns>
        bool SetConfigurationSetting(string name, string value);

        /// <summary>
        ///     Gets a service reference connection string from the configurationmanager
        /// </summary>
        /// <param name="serviceApplicationName">The app name of the service</param>
        /// <returns>The setting</returns>
        string GetServiceReference(string serviceApplicationName);


        /// <summary>
        ///     Registers a service reference 
        /// </summary>
        /// <param name="serviceAppName">The name of the serviceApplication</param>
        /// <param name="serviceAddress">The address of the service</param>
        /// <returns>True if it succeeded, false if it didn't</returns>
        bool RegisterServiceReference(string serviceAppName, string serviceAddress);

        /// <summary>
        ///     Gets the database connection string to the current  database
        /// 
        ///     No security leaks can happen here since the database is authorised through active directory
        /// </summary>
        /// <param name="connectionName">The connection string name</param>
        /// <returns>The connectionstring</returns>
        DbConnection GetDbConnectionString(string connectionName);

        /// <summary>
        ///     Sets the database connection string
        /// </summary>
        /// <param name="connectionName">Name of the connection you want to update</param>
        /// <param name="datasource">The new datasource value</param>
        /// <param name="initialCatalog">The new initialcatalog value</param>
        /// <returns>True if it succeeded, false if it didn't</returns>
        bool SetDbConnectionString(string connectionName, string datasource, string initialCatalog);
    }
}
