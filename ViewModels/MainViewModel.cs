using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using BookManager.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Controls;

namespace BookManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<string> CategoryStats { get; set; } = new ObservableCollection<string>();

        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Today;
        public string Category { get; set; }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                if (value != null)
                {
                    Title = value.Title;
                    Author = value.Author;
                    Publisher = value.Publisher;
                    PublishedDate = value.PublishedDate;
                    Category = value.Category;
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(Author));
                    OnPropertyChanged(nameof(Publisher));
                    OnPropertyChanged(nameof(PublishedDate));
                    OnPropertyChanged(nameof(Category));
                }
                OnPropertyChanged();
            }
        }

        // 필터 관련
        private ICollectionView _filteredBooks;
        public ICollectionView FilteredBooks
        {
            get => _filteredBooks;
            set
            {
                _filteredBooks = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilteredBooks?.Refresh();
            }
        }

        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                FilteredBooks?.Refresh();
            }
        }

        private bool FilterBooks(object obj)
        {
            if (obj is Book book)
            {
                bool matchesSearch = string.IsNullOrWhiteSpace(SearchText) || book.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
                bool matchesCategory = string.IsNullOrWhiteSpace(SelectedCategory) || book.Category == SelectedCategory;

                return matchesSearch && matchesCategory;
            }
            return false;
        }

        public ObservableCollection<Book> RecentBooks { get; set; } = new ObservableCollection<Book>();

        private void UpdateRecentBooks()
        {
            var recent = Books
                .OrderByDescending(b => b.PublishedDate)
                .Take(5)
                .ToList();

            RecentBooks.Clear();
            foreach (var book in recent)
            {
                RecentBooks.Add(book);
            }
        }

        // 명령어들
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public ICommand ExportCsvCommand { get; }
        public ICommand ClearAllCommand { get; }
        public ICommand PrintCommand { get; }
        //  생성자
        public MainViewModel()
        {
            using (var db = new BookDbContext())
            {
                db.Database.EnsureCreated();
                Books = new ObservableCollection<Book>(db.Books.ToList());
            }
                        UpdateCategoryStats();
            // 필터 뷰 구성
            FilteredBooks = CollectionViewSource.GetDefaultView(Books);
            FilteredBooks.Filter = FilterBooks;

            // 카테고리 목록 초기화
            Categories = new ObservableCollection<string>(Books.Select(b => b.Category).Distinct());

            // 명령어 연결
            AddCommand = new RelayCommand(_ => AddBook());
            UpdateCommand = new RelayCommand(_ => UpdateBook(), _ => SelectedBook != null);
            DeleteCommand = new RelayCommand(_ => DeleteBook(), _ => SelectedBook != null);
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());
            ExportCsvCommand = new RelayCommand(_ => ExportToCsv());
            ClearAllCommand = new RelayCommand(_ => ClearAllBooks());
            PrintCommand = new RelayCommand(_ => PrintBooks());

            //시작할 때 업데이트
            UpdateCategoryStats();
            UpdateRecentBooks();
        }

        private void AddBook()
        {
            var book = new Book
            {
                Title = Title,
                Author = Author,
                Publisher = Publisher,
                PublishedDate = PublishedDate,
                Category = Category
            };

            using (var db = new BookDbContext())
            {
                db.Books.Add(book);
                db.SaveChanges();
            }

            Books.Add(book);

            if (!Categories.Contains(book.Category))
            {
                Categories.Add(book.Category);
            }

            ClearInputs();
            FilteredBooks?.Refresh();
            UpdateCategoryStats();
            UpdateRecentBooks();
        }

        private void UpdateBook()
        {
            if (SelectedBook == null) return;

            string oldCategory = SelectedBook.Category;

            using (var db = new BookDbContext())
            {
                var book = db.Books.Find(SelectedBook.Id);
                if (book != null)
                {
                    book.Title = Title;
                    book.Author = Author;
                    book.Publisher = Publisher;
                    book.PublishedDate = PublishedDate;
                    book.Category = Category;
                    db.SaveChanges();
                }
            }

            SelectedBook.Title = Title;
            SelectedBook.Author = Author;
            SelectedBook.Publisher = Publisher;
            SelectedBook.PublishedDate = PublishedDate;
            SelectedBook.Category = Category;

            // 카테고리 추가 (새 카테고리가 없으면 추가)
            if (!Categories.Contains(Category))
                Categories.Add(Category);

            // 이전 카테고리를 아무도 안 쓰면 제거
            if (!Books.Any(b => b.Category == oldCategory))
                Categories.Remove(oldCategory);

            FilteredBooks?.Refresh();
            UpdateCategoryStats();
            UpdateRecentBooks();
        }

        private void DeleteBook()
        {
            if (SelectedBook == null) return;

            string deletedCategory = SelectedBook.Category;

            using (var db = new BookDbContext())
            {
                var book = db.Books.Find(SelectedBook.Id);
                if (book != null)
                {
                    db.Books.Remove(book);
                    db.SaveChanges();
                }
            }

            Books.Remove(SelectedBook);
            SelectedBook = null;

            // 삭제 후, 해당 카테고리가 Books에 더 이상 없으면 Categories에서도 제거
            if (!Books.Any(b => b.Category == deletedCategory))
            {
                Categories.Remove(deletedCategory);
            }

            FilteredBooks?.Refresh();
            UpdateCategoryStats();
            UpdateRecentBooks();
        }

        private void ClearFilter()
        {
            SearchText = string.Empty;
            SelectedCategory = null;
            FilteredBooks?.Refresh();
        }

        private void ClearInputs()
        {
            Title = string.Empty;
            Author = string.Empty;
            Publisher = string.Empty;
            PublishedDate = DateTime.Today;
            Category = string.Empty;

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Author));
            OnPropertyChanged(nameof(Publisher));
            OnPropertyChanged(nameof(PublishedDate));
            OnPropertyChanged(nameof(Category));
        }

        // csv로 내보내기
        private void ExportToCsv()
        {
            var dialog = new SaveFileDialog
            {
                FileName = "books.csv",
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (var writer = new StreamWriter(dialog.FileName))
                    {
                        writer.WriteLine("제목,저자,출판사,출판일,카테고리");

                        foreach (Book book in FilteredBooks.Cast<Book>())
                        {
                            string line = $"{book.Title},{book.Author},{book.Publisher},{book.PublishedDate:yyyy-MM-dd},{book.Category}";
                            writer.WriteLine(line);
                        }
                    }

                    MessageBox.Show("CSV 파일이 저장되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("저장 중 오류 발생: " + ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 전체 삭제
        private void ClearAllBooks()
        {
            if (MessageBox.Show("모든 도서를 삭제하시겠습니까?", "전체 삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (var db = new BookDbContext())
                {
                    db.Books.RemoveRange(db.Books);
                    db.SaveChanges();
                }

                Books.Clear();
                Categories.Clear();
                FilteredBooks?.Refresh();
                SelectedBook = null;
                UpdateCategoryStats();
                UpdateRecentBooks();
            }
        }

        //통계 업데이트
        private void UpdateCategoryStats()
        {
            var stats = Books
                .GroupBy(b => b.Category)
                .Select(g => $"{g.Key}: {g.Count()}권")
                .ToList();

            App.Current.Dispatcher.Invoke(() =>
            {
                CategoryStats.Clear();
                foreach (var stat in stats)
                {
                    CategoryStats.Add(stat);
                }
            });
        }

        private void PrintBooks()
        {
            FlowDocument doc = new FlowDocument();

            doc.PagePadding = new Thickness(40);
            doc.ColumnWidth = double.PositiveInfinity; // 한 줄로

            // 제목
            Paragraph header = new Paragraph(new Run("📚 도서 목록"));
            header.FontSize = 20;
            header.FontWeight = FontWeights.Bold;
            header.TextAlignment = TextAlignment.Center;
            doc.Blocks.Add(header);

            // 도서 목록
            foreach (var book in FilteredBooks.Cast<Book>())
            {
                var p = new Paragraph();
                p.Inlines.Add(new Run($"• {book.Title} / {book.Author} / {book.Publisher} / {book.PublishedDate:yyyy-MM-dd} / {book.Category}"));
                doc.Blocks.Add(p);
            }

            // 프린트 대화창
            PrintDialog dlg = new PrintDialog();
            if (dlg.ShowDialog() == true)
            {
                IDocumentPaginatorSource idpSource = doc;
                dlg.PrintDocument(idpSource.DocumentPaginator, "도서 목록 출력");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
