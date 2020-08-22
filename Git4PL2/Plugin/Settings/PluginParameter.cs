using Castle.Components.DictionaryAdapter.Xml;
using Castle.DynamicProxy.Generators;
using Git4PL2.Plugin.Abstract;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Git4PL2.Plugin.Settings
{
    public class PluginParameter<T> :IPluginParameter
    {
        public ePluginParameterNames Name { get; private set; }
        public ePluginParameterGroupType Group { get; set; }
        public ePluginParameterUIType ParamterType { get; set; }


        public string Description { get; set; }
        public string DescriptionExt { get; set; }

        public T Value { get; private set; }


        public int OrderPosition { get; set; }


        public PluginParameter(ePluginParameterNames parameterName, T DefaultValue)
        {
            Seri.Log.Here().Verbose($"Настройка параметра {parameterName}");

            Name = parameterName;

            var ConstantName = parameterName.ToString();

            try
            {
                // Создаём параметр в настройках приложения
                if (Properties.Settings.Default.Properties[ConstantName] == null)
                {
                    // Создаём атрибут, что это пользовательский параметр
                    SettingsAttributeDictionary Attributes = new SettingsAttributeDictionary();
                    Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());

                    // Провайдер по умолчанию где будет сохранены настройки
                    var provider = Properties.Settings.Default.Providers["LocalFileSettingsProvider"];

                    // Создаем параметр
                    var ParamSettingsProperty = new SettingsProperty(ConstantName, typeof(T), provider, false, DefaultValue, SettingsSerializeAs.String, Attributes, false, false);
                    // Добавляем в настройки
                    Properties.Settings.Default.Properties.Add(ParamSettingsProperty);

                    // Создаём значение параметра
                    var ParamSettingsPropertyValue = new SettingsPropertyValue(ParamSettingsProperty);
                    // Добавляем в настройки
                    Properties.Settings.Default.PropertyValues.Add(ParamSettingsPropertyValue);

                    // Перезагружаем настройки. Так как мы добавили параметр, он теперь считается из файла настроек с диска (или возьмет значение по умолчанию)
                    Properties.Settings.Default.Reload();
                }

                Value = (T)Properties.Settings.Default[ConstantName];
            }
            catch(Exception ex)
            {
                Seri.LogException(ex);
                throw ex;
            }

            if (typeof(T) == typeof(string))
            {
                ParamterType = ePluginParameterUIType.Text;
            } 
            else if (typeof(T) == typeof(bool))
            {
                ParamterType = ePluginParameterUIType.CheckBox;
            } 
            else if (typeof(T) == typeof(int))
            {
                ParamterType = ePluginParameterUIType.Number;
            }

            Seri.Log.Here().Verbose($"Значение параметра {Name}: {Value}");
        }


        public P GetValue<P>()
        {
            if (Value is P answerValue)
                return answerValue;

            throw new NotImplementedException();
        }

        public void SetValue<P>(P value)
        {
            Seri.Log.Here().Verbose($"Патыемся сохранить праметр {Name}, значение: {value}");
            if (value is T SetValue)
            {
                Value = SetValue;
                Properties.Settings.Default[Name.ToString()] = value;

                Seri.Log.Here().Verbose($"Сохранияем праметр {Name}, значение: {value}");
                Properties.Settings.Default.Save();
            }
        }

        public string ValueString
        {
            get
            {
                return GetValue<string>();
            }
            set
            {
                SetValue(value);
            }
        }

        public bool ValueBool
        {
            get
            {
                return GetValue<bool>();
            }
            set
            {
                SetValue(value);
            }
        }

        public int ValueInt
        {
            get
            {
                return GetValue<int>();
            }
            set
            {
                SetValue(value);
            }
        }
    }
}
