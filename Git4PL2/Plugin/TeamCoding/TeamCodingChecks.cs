using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Git4PL2.Plugin.WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.TeamCoding
{
    class TeamCodingChecks : ITeamCodingChecks
    {
        private readonly IIDEProvider _IDEProvider;
        private readonly ISettings _Settings;
        private ITeamCodingProvider _TeamCodingProvider;

        public TeamCodingChecks(IIDEProvider IDEProvider, ISettings Settings)
        {
            _IDEProvider = IDEProvider;
            _Settings = Settings;
        }


        public eTeamCodingChecksResult CheckBeforeCompile(out string ErrorMsg)
        {
            eTeamCodingChecksResult answer = eTeamCodingChecksResult.None;
            ErrorMsg = string.Empty;

            Seri.Log.Here().Debug("Начинаем проверку перед компиляцией");

            // Проверяем включен ли TeamCoding
            if (!_Settings.TEAMCODING_ENABLE)
            {
                Seri.Log.Here().Debug("TeamCoding отключен, пропускаем все проверки");
                answer |= eTeamCodingChecksResult.Allow;
            }
            else
            {
                // Пробуем загрузить провайдера TeamCoding
                try
                {
                    if (_TeamCodingProvider == null)
                        _TeamCodingProvider = NinjectCore.Get<ITeamCodingProvider>();
                }
                catch(Exception ex)
                {
                    // При загрузке провайдера были ошибки
                    Seri.LogException(ex);
                    ErrorMsg = ex.Message;

                    // Запрещаем компилировать объект
                    answer |= eTeamCodingChecksResult.Restrict | eTeamCodingChecksResult.ProviderNotSet;
                    return answer;
                }

                // Текущий сервер
                var ServerName = _IDEProvider.SQLQueryExecute<DummyString>(_Settings.SQL_SERVERNAME)[0].Value;
                Seri.Log.Here().Debug("ServerName = " + ServerName);

                // Текущий объект
                var dbObject = _IDEProvider.GetDbObject<DbObject>(true);
                Seri.Log.Here().Debug($"dbObject {dbObject}");

                if (dbObject == null)
                {
                    // Если объет не определн, пропускам
                    answer |= eTeamCodingChecksResult.Allow;
                    Seri.Log.Here().Warning("Перед компиляцией не смогли определить объект, почему? (Не должны здесть оказаться)");
                }
                else
                {
                    // Текущий владелец объекта по версии Team Coding-а
                    var UserOwner = _TeamCodingProvider.GetUserOwner(dbObject, ServerName);
                    Seri.Log.Here().Debug("UserOwner = " + UserOwner);

                    // Владельца нет
                    if (string.IsNullOrEmpty(UserOwner))
                    {
                        // Проверяем можем ли мы компилировать без CheckOut
                        if (_Settings.TEAMCODING_RESTRICT_COMPILE_WITHOUT_CHECKOUT)
                        {
                            ErrorMsg = "Запрещено компилировать объект без CheckOut";
                            answer |= eTeamCodingChecksResult.Restrict;
                        }
                        else
                        {
                            answer |= eTeamCodingChecksResult.Allow;
                        }
                    }
                    else
                    {
                        // Владелец есть, и это МЫ
                        if (UserOwner == _Settings.TEAMCODING_LOGIN)
                        {
                            answer |= eTeamCodingChecksResult.Allow;
                        }
                        else
                        {
                            ErrorMsg = $"Невозможно скомпилировать. Объект находится в пользовании [{UserOwner}]";
                            answer |= eTeamCodingChecksResult.Restrict;
                        }
                    }
                }
            }

            return answer;
        }

        public eTeamCodingChecksResult CheckBeforeOpen(out string ErrorMsg)
        {
            eTeamCodingChecksResult answer = eTeamCodingChecksResult.None;
            ErrorMsg = string.Empty;

            Seri.Log.Here().Debug("Начинаем проверку перед открытием объекта");

            // Проверяем включен ли TeamCoding
            if (!_Settings.TEAMCODING_ENABLE)
            {
                Seri.Log.Here().Debug("TeamCoding отключен, пропускаем все проверки");
                answer |= eTeamCodingChecksResult.Allow;
            }
            else
            {
                // Пробуем загрузить провайдера TeamCoding
                try
                {
                    if (_TeamCodingProvider == null)
                        _TeamCodingProvider = NinjectCore.Get<ITeamCodingProvider>();
                }
                catch(Exception ex)
                {
                    // При загрузке провайдера были ошибки
                    Seri.LogException(ex);
                    ErrorMsg = ex.Message;
                    // Запрещаем компилировать объект
                    answer |= eTeamCodingChecksResult.Allow | eTeamCodingChecksResult.ProviderNotSet;
                    return answer;
                }

                // Текущий сервер
                var ServerName = _IDEProvider.SQLQueryExecute<DummyString>(_Settings.SQL_SERVERNAME)[0].Value;
                Seri.Log.Here().Debug("ServerName = " + ServerName);

                // Текущий объект
                var dbObject = _IDEProvider.GetDbObject<DbObject>(true);
                Seri.Log.Here().Debug($"dbObject {dbObject}");

                if (dbObject == null)
                {
                    answer |= eTeamCodingChecksResult.Allow;
                }
                else
                {
                    var UserOwner = _TeamCodingProvider.GetUserOwner(dbObject, ServerName);
                    Seri.Log.Here().Debug("UserOwner = " + UserOwner);
                    if (!string.IsNullOrEmpty(UserOwner))
                    {
                        ErrorMsg = $"Объект находится в пользовании у [{UserOwner}]";
                        answer |= eTeamCodingChecksResult.Restrict;
                    }
                }
            }

            return answer;
        }
    }
}
