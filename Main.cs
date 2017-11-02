using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using Wox.Plugin;

namespace WoxPlugin_NS
{
    public class Main : IPlugin {
        public void Init(PluginInitContext context) { }
        
        public List<Result> Query(Query query) {
            List<Result> results = new List<Result>();
            if (query.Search.Length == 0) {
                results.Add(new Result() {
                    Title = "NS Lookup",
                    SubTitle = "NS Lookup Plugin - zguo",
                    IcoPath = "images\\app.ico",
                    Action = e => {
                        return false;
                    }
                });
            } else {
                string[] resultstrings = Nslookup.DNSLookup(query.Search);

                for (int i = 1; i < resultstrings.Length; i++) {
                    var index = i;
                    results.Add(new Result() {
                        Title = (Regex.Match(resultstrings[0], query.Search + "(\\.|$)").Success || Regex.Match(query.Search, "\\d+\\.\\d+\\.\\d+\\.\\d+").Success) ? 
                                    resultstrings[0] : query.Search + " => " + resultstrings[0],
                        SubTitle = resultstrings[index],
                        IcoPath = "images\\app.ico",
                        Action = e => {
                            return false;
                        }
                    });
                }
            }
            
            return results;
        }
    }

    class Nslookup {
        public static string[] DNSLookup(string hostNameOrIP) {
            try {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostNameOrIP);
                IPAddress[] IPs = hostEntry.AddressList;
                if (IPs.Length > 0) {
                    string[] results = new string[IPs.Length + 1];
                    results[0] = hostEntry.HostName;
                    for (int i = 1; i < IPs.Length + 1; i++) {
                        var index = i;
                        results[index] = IPs[index-1].ToString();
                    }
                    return results;
                } else {
                    string[] results = new string[] { " " };
                    return results;
                }
            } catch {
                string[] results = new string[] { " " };
                return results;
            }
        }
    }
}
