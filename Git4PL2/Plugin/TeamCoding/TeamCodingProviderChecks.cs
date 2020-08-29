using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.TeamCoding
{
    class TeamCodingProviderChecks : ITeamCodingProviderChecks
    {
        private ISettings _Settings;

        public TeamCodingProviderChecks(ISettings Settings)
        {
            _Settings = Settings;
        }

        public bool ChecksBeforeCheckIn(IEnumerable<ICheckOutObject> list, ICheckOutObject checkInObject, out string ErrorMsg)
        {
            if (!CheckServer(checkInObject.ServerName, out ErrorMsg))
                return false;

            if (!list.Contains(checkInObject))
            {
                var objName = $"{checkInObject.ObjectType} {checkInObject.ObjectOwner}.{checkInObject.ObjectName}";
                ErrorMsg = $"В настоящий момент вы не являетесь владельцем {objName}";
                return false;
            }

            return true;
        }

        public bool ChecksBeforeCheckOut(IEnumerable<ICheckOutObject> list, ICheckOutObject checkOutObject, out string ErrorMsg)
        {
            if (!CheckServer(checkOutObject.ServerName, out ErrorMsg))
                return false;

            if (list.Any(x=> x.ServerName == checkOutObject.ServerName
                          && x.ObjectName == checkOutObject.ObjectName
                          && x.ObjectOwner == checkOutObject.ObjectOwner
                          && x.ObjectType == checkOutObject.ObjectType))
            {
                var ExistedCheckoutLogin = list.First(x => x.ServerName == checkOutObject.ServerName
                                                        && x.ObjectName == checkOutObject.ObjectName
                                                        && x.ObjectOwner == checkOutObject.ObjectOwner
                                                        && x.ObjectType == checkOutObject.ObjectType).Login;
                ErrorMsg = $"Невозможно сделать CheckOut, объект находится в пользовании у {ExistedCheckoutLogin}";
                return false;
            }

            return true;
        }

        private bool CheckServer(string Server, out string ErrorMsg)
        {
            if (!Regex.IsMatch(Server, _Settings.TEAMCODING_SERVERNAME_REGEX))
            {
                ErrorMsg = $"Текущий сервер [{Server}] не прошел проверку [{_Settings.TEAMCODING_SERVERNAME_REGEX}]";
                return false;
            }
            {
                ErrorMsg = string.Empty;
                return true;
            }
        }
    }
}
