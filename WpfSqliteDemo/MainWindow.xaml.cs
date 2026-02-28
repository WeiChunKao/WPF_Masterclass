using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfSqliteDemo.Data;
using WpfSqliteDemo.Models;

namespace WpfSqliteDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        _ = RefreshAsync();
    }

    private AppDbContext NewDb() => new(App.DbOptions);

    private async Task RefreshAsync()
    {
        await using var db = NewDb();
        var items = await db.Todos
            .OrderByDescending(x => x.Id)
            .AsNoTracking()
            .ToListAsync();

        GridTodos.ItemsSource = items;
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e)
    {
        await RefreshAsync();
    }

    private async void Add_Click(object sender, RoutedEventArgs e)
    {
        var title = (TitleBox.Text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            MessageBox.Show("Title is required.");
            return;
        }

        await using var db = NewDb();
        db.Todos.Add(new TodoItem { Title = title });
        await db.SaveChangesAsync();

        TitleBox.Text = "";
        await RefreshAsync();
    }

    private async void SaveRow_Click(object sender, RoutedEventArgs e)
    {
        if (GridTodos.SelectedItem is not TodoItem row) return;

        // Grid 綁的是 AsNoTracking 的結果，因此用 Update 直接覆寫
        await using var db = NewDb();
        db.Todos.Update(row);
        await db.SaveChangesAsync();

        await RefreshAsync();
    }

    private async void DeleteRow_Click(object sender, RoutedEventArgs e)
    {
        if (GridTodos.SelectedItem is not TodoItem row) return;

        await using var db = NewDb();
        db.Todos.Remove(new TodoItem { Id = row.Id }); // 不用先查
        await db.SaveChangesAsync();

        await RefreshAsync();
    }
}
