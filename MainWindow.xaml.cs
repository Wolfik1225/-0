using System.Buffers.Text;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace WpfApp50
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Triangle tr;
        Random rnd = new Random();
        List<EnemyIcon> enemyIcons = new List<EnemyIcon>();
        CEnemyTemplateList enemyList = new CEnemyTemplateList();
        string selectedIconName = "";
        Point2D baseP1, baseP2, baseP3; // запомненные "родные" координаты 
        Point2D baseRectStart;
        int baseRectW, baseRectH;
        Rectangle rect;
        public MainWindow()
        {
            InitializeComponent();
            Point2D p1 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p2 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p3 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            tr = new Triangle(p1, p2, p3);
            DrawTriangle(tr);
            Rectangle rect = new Rectangle(new Point2D(50, 50), 200, 100);
            DrawRectangle(rect);
            Rectangle square = new Rectangle(new Point2D(100, 155), 150, 150);
            DrawRectangle(square);
        }
        public void DrawLine(Point2D p1, Point2D p2)
        {
            Line line = new Line();
            line.Stroke = Brushes.Red;
            line.StrokeThickness = 3;

            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;

            Scene.Children.Add(line);
        }

        public void DrawTriangle(Triangle tr)
        {
            DrawLine(tr.P1, tr.P2);
            DrawLine(tr.P2, tr.P3);
            DrawLine(tr.P3, tr.P1);
        }

        public void ClearScene()
        {
            Scene.Children.Clear();
        }

        public void DrawRectangle(Rectangle rect)
        {
            // Вычисляем 4 угла прямоугольника из стартовой точки, ширины и высоты
            Point2D topLeft = rect.Start;
            Point2D topRight = new Point2D(rect.Start.X + rect.Width, rect.Start.Y);
            Point2D bottomRight = new Point2D(rect.Start.X + rect.Width, rect.Start.Y + rect.Height);
            Point2D bottomLeft = new Point2D(rect.Start.X, rect.Start.Y + rect.Height);

            // Соединяем углы линиями по кругу
            DrawLine(topLeft, topRight);
            DrawLine(topRight, bottomRight);
            DrawLine(bottomRight, bottomLeft);
            DrawLine(bottomLeft, topLeft);
        }
        private void BtnUserTriangle_Click(object sender, RoutedEventArgs e)
        {
            ClearScene();
            Point2D p1 = new Point2D(int.Parse(TxtX1.Text), int.Parse(TxtY1.Text));
            Point2D p2 = new Point2D(int.Parse(TxtX2.Text), int.Parse(TxtY2.Text));
            Point2D p3 = new Point2D(int.Parse(TxtX3.Text), int.Parse(TxtY3.Text));
            tr = new Triangle(p1, p2, p3);
            baseP1 = p1; baseP2 = p2; baseP3 = p3;
            DrawTriangle(tr);
        }

        private void BtnRandomTriangle_Click(object sender, RoutedEventArgs e)
        {
            ClearScene();
            Point2D p1 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p2 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p3 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            tr = new Triangle(p1, p2, p3);
            baseP1 = p1; baseP2 = p2; baseP3 = p3;
            DrawTriangle(tr);
        }

        private void BtnRandomRect_Click(object sender, RoutedEventArgs e)
        {
            ClearScene();
            Point2D start = new Point2D(rnd.Next(0, (int)Scene.Width / 2), rnd.Next(0, (int)Scene.Height / 2));
            int w = rnd.Next(50, 200);
            int h = rnd.Next(50, 150);
            Rectangle rect = new Rectangle(start, w, h);

            baseRectStart = start;
            baseRectW = w;
            baseRectH = h;
            DrawRectangle(rect);
        }

        private void BtnSquare_Click(object sender, RoutedEventArgs e)
        {
            ClearScene();
            Point2D start = new Point2D(rnd.Next(0, (int)Scene.Width / 2), rnd.Next(0, (int)Scene.Height / 2));
            int side = rnd.Next(50, 150);
            Rectangle square = new Rectangle(start, side, side);
            baseRectStart = start;
            baseRectW = side;
            baseRectH = side;
            DrawRectangle(square);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearScene();
        }
        private void SliderX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MoveShapes();
        }

        private void SliderY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MoveShapes();
        }

        private void MoveShapes()
        {
            int dx = (int)SliderX.Value;
            int dy = (int)SliderY.Value;
            ClearScene();

            if (baseP1 != null)
            {
                Point2D newP1 = new Point2D(baseP1.X + dx, baseP1.Y + dy);
                Point2D newP2 = new Point2D(baseP2.X + dx, baseP2.Y + dy);
                Point2D newP3 = new Point2D(baseP3.X + dx, baseP3.Y + dy);

                tr = new Triangle(newP1, newP2, newP3);
                DrawTriangle(tr);
            }

            if (baseRectStart != null)
            {
                Point2D newStart = new Point2D(baseRectStart.X + dx, baseRectStart.Y + dy);
                rect = new Rectangle(newStart, baseRectW, baseRectH);
                DrawRectangle(rect);
            }
        }

        public void LoadIconsFromFolder(string path)
        {
            // фильтр расширения изображения
            string filter = "*.png";

            // получение массива строк, содержащих пути до изображений
            string[] files = Directory.GetFiles(path, filter);

            // перебор всех полученных путей
            // в file содержится путь до изображения с расширением .png
            foreach (string file in files)
            {
                enemyIcons.Add(
                    new EnemyIcon
                    {
                        // получение имени файла с расширением
                        Name = System.IO.Path.GetFileName(file),
                        // получение полного пути до файла
                        ImagePath = file
                    }
                );
            }
        }
        public void DisplayIcons()
        {
            IconsListBox.Items.Clear(); // на случай повторной загрузки — чистим старый список

            foreach (EnemyIcon icon in enemyIcons)
            {
                // создание элемента Image для отображения иконки
                Image image = new Image
                {
                    // установка источника изображения
                    Source = new BitmapImage(
                        new Uri(icon.ImagePath) // Uri — это универсальный идентификатор ресурса,
                                                // который указывает на местоположение ресурса,
                                                // в данном случае на путь к файлу изображения
                    ),
                    Height = 64 // высота изображения
                };
                IconsListBox.Items.Add(image); // добавление изображения в ListBox
            }
        }

        private void BtnLoadIcons_Click(object sender, RoutedEventArgs e)
        {
            // создание диалога выбора папки
            var dlg = new Microsoft.Win32.OpenFolderDialog();

            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FolderName;
                LoadIconsFromFolder(path);
                DisplayIcons();
            }
        }
        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // приведение sender к типу ListBox
            ListBox iconHolder = sender as ListBox;

            // проверка, что выбранный элемент является изображением
            // и что элемент не равен null
            if (iconHolder.SelectedItem is Image selectedImage && iconHolder.SelectedItem != null)
            {
                // получение имени файла из источника изображения
                // так как Source это Uri, то для получения имени файла
                // нужно преобразовать его в строку и использовать Path.GetFileName
                string iconName = System.IO.Path.GetFileName(selectedImage.Source.ToString());

                // сохраняем выбранное имя иконки в поле — пригодится при создании противника
                selectedIconName = iconName;
            }
        }
        private void BtnAddEnemy_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtEnemyName.Text;
            int baseLife = int.Parse(TxtBaseLife.Text);
            double lifeModifier = double.Parse(TxtLifeModifier.Text);
            int baseGold = int.Parse(TxtBaseGold.Text);
            double goldModifier = double.Parse(TxtGoldModifier.Text);
            double spawnChance = double.Parse(TxtSpawnChance.Text);

            enemyList.AddEnemy(name, selectedIconName, baseLife, lifeModifier,
                                baseGold, goldModifier, spawnChance);

            RefreshEnemiesListBox();
        }

        private void BtnRemoveEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (EnemiesListBox.SelectedIndex >= 0)
            {
                enemyList.DeleteEnemyByIndex(EnemiesListBox.SelectedIndex);
                RefreshEnemiesListBox();
            }
        }

        private void RefreshEnemiesListBox()
        {
            EnemiesListBox.Items.Clear();
            foreach (string name in enemyList.GetListOfNames())
            {
                EnemiesListBox.Items.Add(name);
            }
        }
    }
}