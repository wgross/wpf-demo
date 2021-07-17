using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace DemoApp.Translation
{
//    public sealed class TranslationManager
//    {
//        #region Construction and initialization of this instance

//        public TranslationManager()
//        {
//            this.cultures = new Lazy<IEnumerable<CultureInfo>>(() => CultureInfo.GetCultures(CultureTypes.SpecificCultures), isThreadSafe: true);
//        }

//        private readonly Lazy<IEnumerable<CultureInfo>> cultures;

//        #endregion Construction and initialization of this instance

//        #region Singleton

//        public static readonly TranslationManager Current = new TranslationManager();

//        #endregion Singleton

//        /// <summary>
//        /// Reads or sets the threads current Ui culture.
//        /// Triggers on language changed event
//        /// </summary>
//        public CultureInfo CurrentUICulture
//        {
//            get => Thread.CurrentThread.CurrentUICulture;
//            set
//            {
//                if (value != Thread.CurrentThread.CurrentUICulture)
//                {
//                    UICultureChangedEvent.SetCurrentUICulture(value);
//                }
//            }
//        }

//        private IEnumerable<CultureInfo> Cultures => cultures.Value;
//    }
}