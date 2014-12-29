﻿using System.Windows;
using System.Collections.Generic;
using UsingTask.Library;
using UsingTask.Shared;
using System.Threading.Tasks;
using System.Threading;

namespace UsingTask.UI
{
    public partial class MainWindow : Window
    {
        PersonRepository repository = new PersonRepository();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
        {
            FetchWithTaskButton.IsEnabled = false;
            ClearListBox();
            Task<List<Person>> peopleTask = repository.Get();
            peopleTask.ContinueWith(t =>
                {
                    List<Person> people = t.Result;
                    foreach (var person in people)
                        PersonListBox.Items.Add(person);
                },
                new CancellationToken(),
                TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.FromCurrentSynchronizationContext());

            peopleTask.ContinueWith(t => FetchWithTaskButton.IsEnabled = true,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
        {
            FetchWithAwaitButton.IsEnabled = false;
            try
            {
                ClearListBox();
                List<Person> people = await repository.Get();
                foreach (var person in people)
                    PersonListBox.Items.Add(person);

            }
            finally
            {
                FetchWithAwaitButton.IsEnabled = true;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();
        }

        private void ClearListBox()
        {
            PersonListBox.Items.Clear();
        }
    }
}
