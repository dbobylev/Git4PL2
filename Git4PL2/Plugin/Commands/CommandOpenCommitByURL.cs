using Git4PL2.Plugin.Abstract;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandOpenCommitByURL : PluginCommand
    {
        private readonly ISettings _Settings;

        public CommandOpenCommitByURL(ISettings Settings) : base("PluginCommandOpenCommitByURL")
        {
            _Settings = Settings;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null)
                throw new Exception("Не указан SHA комита");

            var shaCommit = parameter.ToString();
            var URL = _Settings.CommitViewURL;

            Seri.Log.Here().Verbose("Пытаемся открыть ссылку для комита: " + shaCommit);
            System.Diagnostics.Process.Start(URL + shaCommit);
        }

        public override bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                return !string.IsNullOrEmpty(parameter.ToString());
            }
            return false;
        }
    }
}
