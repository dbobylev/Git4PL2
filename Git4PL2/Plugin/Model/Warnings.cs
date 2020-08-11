using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Model
{
    public class Warnings : IWarnings
    {
        public bool IsBranchUnexsepted(string BranchName, bool SilentMode = false)
        {
            if (Properties.Settings.Default.UnexpectedBranch)
            {
                string RegexInPattern = Properties.Settings.Default.WarnInRegEx;
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
            if (Properties.Settings.Default.UnexpectedServer)
            {
                string RegexOutPattern = Properties.Settings.Default.WarnOutRegEx;
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
