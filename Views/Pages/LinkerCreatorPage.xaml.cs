using System.IO;

namespace DirLink.Views.Pages;
/// <summary>
/// Interaction logic for LinkerPage.xaml
/// </summary>
public partial class LinkerCreatorPage {
    public LinkerCreatorPage() {
        InitializeComponent();
        DataContext = this;
    }

    /// <summary>
    /// Gets or sets the desired linker destination folder.
    /// </summary>
    /// <value>
    /// The desired linker destination folder.
    /// </value>
    public DirectoryInfo? Destination { get; set; }
}
