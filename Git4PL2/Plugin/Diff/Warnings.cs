using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Diff
{
    public class Warnings : IWarnings
    {
        private readonly ISettings _Settings;

        public Warnings(ISettings Settings)
        {
            _Settings = Settings;
        }

        public bool IsBranchUnexsepted(string BranchName, bool SilentMode = false)
        {
            if (_Settings.UnexpectedBranch)
            {
                string RegexInPattern = _Settings.WarnInRegEx;
                Regex regex = new Regex(RegexInPattern);
                if (!regex.IsMatch(BranchName))
                {
                    if (SilentMode)
                        return true;
                    MessageBoxResult result = MessageBox.Show($"Внимание! Вы работаете с веткой: {BranchName}. Продолжить?"
                        , $"Название ветки не прошло проверку", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                        return true;
                }
            }
            return false;
        }

        public bool IsServerUnexsepted(string ServerName, bool SilentMode = false)
        {
            if (_Settings.UnexpectedServer)
            {
                string RegexOutPattern = _Settings.WarnOutRegEx;
                Regex regex = new Regex(RegexOutPattern, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(ServerName))
                {
                    if (SilentMode)
                        return true;
                    MessageBoxResult result = MessageBox.Show($"Внимание! Вы собираетесь изменить текст объекта на сервере: {ServerName}. Продолжить?"
                        , $"Название сервера не прошло проверку", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                        return true;
                }
            }
            return false;
        }
    }
}
