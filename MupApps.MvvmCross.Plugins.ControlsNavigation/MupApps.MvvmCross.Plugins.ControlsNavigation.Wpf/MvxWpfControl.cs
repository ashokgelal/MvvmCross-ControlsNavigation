// MvxStoreControl.cs
// (c) Copyright Christian Ruiz @_christian_ruiz
// MvvmCross - Controls Navigation Plugin is licensed using Microsoft Public License (Ms-PL)
// 

using System;
using System.ComponentModel;
using System.Windows;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Views;

namespace MupApps.MvvmCross.Plugins.ControlsNavigation.Wpf
{
    public class MvxWpfControl
        : MvxWpfView, IMvxControl, IDisposable
    {
        public new IMvxViewModel ViewModel
        {
            get
            {
                return base.ViewModel;
            }
            set
            {
                base.ViewModel = value; 
                this.CheckEmptyControlBehaviour();
            }
        }

        private readonly IMvxControlsContainer _container;

        public MvxWpfControl()
        {
            DataContext = null;

            EmptyControlBehaviour = this.GetDefaultEmptyControlBehaviour();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            if (!Mvx.CanResolve<IMvxControlsContainer>())
                new Plugin().Load();

            _container = Mvx.Resolve<IMvxControlsContainer>();

            _container.Add(this);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _container.Remove(this);
        }

        public void ResetControl(Type viewModelType)
        {
            _container.Reset(viewModelType);
        }

        private EmptyControlBehaviours? _emptyControlBehaviour;
        private bool _disposed;

        public EmptyControlBehaviours EmptyControlBehaviour
        {
            get
            {
                return _emptyControlBehaviour.HasValue
                    ? _emptyControlBehaviour.Value
                    : this.GetDefaultEmptyControlBehaviour();
            }
            set
            {
                var lastBehaviour = _emptyControlBehaviour;
                _emptyControlBehaviour = value;
                this.CheckEmptyControlBehaviour(lastBehaviour);
            }
        }

        public void ChangeVisibility(bool visible)
        {
            Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void ChangeEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                Loaded -= OnLoaded;
                Unloaded -= OnUnloaded; 
            }
            _disposed = true;
        }
    }
}
