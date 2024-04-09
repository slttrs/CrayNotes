using Caliburn.Micro;
using CrayNotes.WPFUI.Models;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace CrayNotes.WPFUI.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string? _noteContents;
		private NoteModel _selectedNote;
        private BindableCollection<NoteModel> _notesList = new BindableCollection<NoteModel>();
        public string? NoteContents
		{
			get { return _noteContents; }
			set
			{
				_noteContents = value;
				NotifyOfPropertyChange(() => NoteContents);
			}
		}
		public NoteModel SelectedNote
		{
			get { return _selectedNote; }
			set
			{
				DoSave();
				_selectedNote = value;
				NotifyOfPropertyChange(() => SelectedNote);
                ShowNoteContent(_selectedNote);
			}
		}
        public BindableCollection<NoteModel> NotesList
        {
            get { return _notesList; }
            set
			{
				_notesList = value;
                NotifyOfPropertyChange(() => NotesList);
            }
        }

        /*
		to do:
		auto save
		save on close
		contact notemodel to:
		- create
		- open
		- save
		- delete
		*/

        public ShellViewModel()
		{
            System.Diagnostics.Trace.WriteLine("Constructing ShellViewModel");

			// get list of notes to work with
            DataAccess.DataAccess da = new DataAccess.DataAccess();
			List<NoteModel> dataAccessNotesList = da.GetNotesList();
            dataAccessNotesList.ForEach(note => NotesList.Add(note));

            System.Diagnostics.Trace.WriteLine("notes list:");
            dataAccessNotesList.ForEach(note => System.Diagnostics.Trace.WriteLine($"element {note.Name}"));
            

            // setting selectednote on startup
            if (NotesList.Count <= 0)
			{
				NotesList.Add(da.CreateNote());
                
            }
            System.Diagnostics.Trace.WriteLine($"NoteList 0: {NotesList[0].Name}");
            SelectedNote = NotesList[0];
            System.Diagnostics.Trace.WriteLine($"Selected Note: {SelectedNote.Name}");
        }

		// load selected note content
		public void ShowNoteContent(NoteModel note)
		{
			if (note != null)
			{
                if (note.Contents != null)
			    {
					NoteContents = note.Contents;
				}
				else
				{
					NoteContents = note.LoadContents();
				}
			}
			
		}


        //private static double autosaveInterval = 3.0;
        //DispatcherTimer autosaveTimer = new DispatcherTimer(TimeSpan.FromSeconds(autosaveInterval), DispatcherPriority.Background, new EventHandler(DoAutoSave), Application.Current.Dispatcher);

        /*private void DoAutoSave(object sender, EventArgs e)
        {
			DoSave();
        }*/

		public void DoSave()
		{
			if (SelectedNote != null)
			{
                System.Diagnostics.Trace.WriteLine($"Saving note");
                SelectedNote.Contents = NoteContents;
				SelectedNote.SaveContents();
			}
        }

		public void CreateNote()
		{
            DataAccess.DataAccess da = new DataAccess.DataAccess();
			NoteModel newNote = da.CreateNote();
			NotesList.Add(newNote);
			SelectedNote = NotesList[^1];
        }
    }
}
