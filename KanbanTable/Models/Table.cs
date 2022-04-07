using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Imaging;

namespace KanbanTable.Models
{
    [Serializable]
    public class Table : INotifyPropertyChanged
    {
        string header;
        string description;
        Bitmap? filePath;

        public string Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value;
                RaisePropertyChangedEvent("Header");
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                RaisePropertyChangedEvent("Description");
            }
        }
        [XmlIgnore]
        public Bitmap? FilePath
        {
            get
            {
                return this.filePath;
            }
            set
            {
                this.filePath = value;
                RaisePropertyChangedEvent("FilePath");
            }
        }

        public Table(string header)
        {
            this.Header = header;
            this.Description = "";
            this.FilePath = null;
        }

        public Table()
        {
            this.Header = "New Task";
            this.Description = "";
            this.FilePath = null;
        }

        public async void UploadFile(Window parent)
        {
            var openFileDialog = new OpenFileDialog().ShowAsync(parent);
            string[]? path = await openFileDialog;
            if (path is not null)
            {
                string sourcePath = String.Join("/", path);
                FileInfo fileInfo = new FileInfo(sourcePath);
                using (FileStream fs = fileInfo.OpenRead())
                {
                    try
                    {
                        this.FilePath = Bitmap.DecodeToWidth(fs, 100);
                    }
                    catch (Exception e)
                    {
                        this.FilePath = null;
                    }
                }
            }
        }

        public void DeleteFile()
        {
            this.FilePath = null;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("FilePath")]
        public byte[] FilePathSerialized
        {
            get
            { // serialize
                if (FilePath == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    FilePath.Save(ms);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    FilePath = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        FilePath = new Bitmap(ms);
                    }
                }
            }
        }

    }
}
