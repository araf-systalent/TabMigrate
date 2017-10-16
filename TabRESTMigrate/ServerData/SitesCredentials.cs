using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SitesCredentials
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string SiteUrl { get; set; }
    public Configuration configuration { get; set; }
}

public class Configuration
{
    public string Path { get; set; }
    public string DbName { get; set; }
    public string Server { get; set; }
    public string Username { get; set; }
    public string password { get; set; }
}


