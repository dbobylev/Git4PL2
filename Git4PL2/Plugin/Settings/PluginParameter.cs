using Castle.Components.DictionaryAdapter.Xml;
using Castle.DynamicProxy.Generators;
using Git4PL2.Plugin.Abstract;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Git4PL2.Plugin.Settings
{
    public class PluginParameter<T> :IPluginParameter
    {
        /// <summary>
        /// ID параметра, используется при сохранении в user.config
        /// </summary>
        public ePluginParameterID ID { get; private set; }

        /// <summary>
        /// Группа к которой относится параметр
        /// </summary>
        public ePluginParameterGroupType Group { get; set; }

        /// <summary>
        /// Тип отображения параметра в окне настроек
        /// </summary>
        public ePluginParameterUIType ParamterUIType { get; set; }

        /// <summary>
        /// Название параметра 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Подробное описание работы параметра
        /// </summary>
        public string DescriptionExt { get; set; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Действие происходит при смене параметра
        /// </summary>
        public Action<T> OnParameterChanged { get; set; }

        /// <summary>
        /// Порядок сортировки параматров в рамках одной группы
        /// </summary>
        public int OrderPosition { get; set; }

        /// <summary>
        /// Ссылка на параметр родителя. 
        /// Если родитель имеет значение отличное от ParentParameterStringValue или отключен, этот параметр становится отключенным 
        /// (т.е. недоступным для редактирования в окне настроек)
        /// Поиск родителя происходит в рамках группы
        /// 
        /// Если ParentParameter не указывать то параметр останется всегда включен 
        /// </summary>
        public ePluginParameterID ParentParameterID { get; set; } = ePluginParameterID.NULL;

        /// <summary>
        /// Значение ParentParameterID при котором этот параметр будет активный на UI
        /// </summary>
        public string ParentParameterStringValue { get; set; } = "True";


        public PluginParameter(ePluginParameterID parameterName, T DefaultValue)
        {
            Seri.Log.Here().Verbose($"Настройка параметра {parameterName}");

            ID = parameterName;

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
                ParamterUIType = ePluginParameterUIType.Text;
            } 
            else if (typeof(T) == typeof(bool))
            {
                ParamterUIType = ePluginParameterUIType.CheckBox;
            } 
            else if (typeof(T) == typeof(int))
            {
                ParamterUIType = ePluginParameterUIType.Number;
            }

            Seri.Log.Here().Verbose($"Значение параметра {ID}: {Value}");
        }
        
        public string GetStringValue => Value.ToString();

        public P GetValue<P>()
        {
            if (typeof(P) == typeof(string))
                return (P)(object)Value;
            if (Value is P answerValue)
                return answerValue;

            
            throw new NotImplementedException();
        }

        public void SetValue<P>(P value)
        {
            Seri.Log.Here().Verbose($"Патыемся сохранить праметр {ID}, значение: {value}");
            if (value is T SetValue)
            {
                Value = SetValue;
                Properties.Settings.Default[ID.ToString()] = value;

                Seri.Log.Here().Verbose($"Сохранияем праметр {ID}, значение: {value}");
                Properties.Settings.Default.Save();

                OnParameterChanged?.Invoke(SetValue);
            }
        }
    }
}
