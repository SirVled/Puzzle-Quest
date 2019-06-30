using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Puzzle_Quest__AnimeEdition_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        public static Random randomGenerator = new Random();

        //Enemy // 
        public int HpValueEn;
        // /Enemy //
        // Hero //

        public int IdHpHero; //Кол-во очков здоровья героя;

        public int IdHero { get; set; } //id Героя; 

        public int[] SpellСhoice = new int[5]; //Выбранные умения героя;

        // /Hero //

        public int MonsterNum { get; set; } // id Монстра

        public MainWindow()
        {
            BotBrain = new BrainBot(this);
            SClass = new SpellClass(this);

            InitializeComponent();

        }

        // Thread myThread;
        // Thread AnimationMovesBlock;

        SpellClass SClass;
        BrainBot BotBrain;
        MainMenu MenuBack;

       // Boolean bl = false;

        public Label[] BFHer = new Label[7]; // Положительные/Отрицательные эффекты героя;
        public Label[] BFEnemy = new Label[7]; // Положительные/Отрицательные эффекты противника;
        public Rectangle[,] Rec = new Rectangle[8, 8]; //Блоки;
        public Rectangle[,] BackgroundRecHover = new Rectangle[8, 8]; //Задний фон блоков;
        public Rectangle[] rememberBlockAnim = new Rectangle[8]; // Запоминает блоки которые используются в анимации;
        
        private Random rnd = new Random();

        public Boolean CheckMouseUp = false; //Служит для проверки на нажатие клавиши мыши;
        private static Boolean NoStackBlocks = false; //Служит для проверки стаков в поле;
        public Boolean StartGame = false; // Служит началу игры;
        public Boolean ExtraTurn = false; // Служит для проверки дополнительного хода;

        public Boolean TurnHeroAndEnemy = true; //Служит для проверки хода (чей ход будет выполнен);

        public Boolean CheckToExtra = false; //Служит для проверки дополнительного хода (2);

        public Thread myThread; //Поток с анимацией который нужно закрыть после закрытия программы;

        private int Rowint = 0;
        private int Colummint = 0;


        private int NumMassRow = 0;
        private int NumMassColumm = 0;


        private Rectangle SGRec { get; set; }


        private Rectangle[] RowRec = new Rectangle[128];
        private Rectangle[] ColummRec = new Rectangle[128];

        private int[] RecRowSG = new int[128];
        private int[] RecColummSG = new int[128];


        //Заполнение игрового поля;
        private void Select_Items(object sender, RoutedEventArgs e)
        {

            Application aplicat = Application.Current;
            aplicat.Exit += Programm_Exit;

            SetBackgroundMainWindow();

            heroL.Content = heroP.Value;
            enemyP.Maximum = HpValueEn;          
            enemyP.Value = enemyP.Maximum;
            enemyL.Content = enemyP.Value;
            // Заполнение поля //
            // Баффы //

            for (int i = 0; i < 7; i++)
            {
                BFHer[i] = CreateBF();
                Grid.SetColumn(BFHer[i], i);

                BuffsHero.Children.Add(BFHer[i]);

                BFEnemy[i] = CreateBF();
                Grid.SetColumn(BFEnemy[i], i);

                BuffsEnemy.Children.Add(BFEnemy[i]);
            }
            // /Баффы //

            // Поле //
            for (int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 8; j++)
                {
                    BackgroundRecHover[i, j] = CreateItemBack();

                    Grid.SetColumn(BackgroundRecHover[i, j], j);
                    Grid.SetRow(BackgroundRecHover[i, j], i);

                    GamePane.Children.Add(BackgroundRecHover[i, j]);

                }
            }
            for (int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 8; j++)
                {
                    Rec[i, j] = CreateItem();

                    Grid.SetColumn(Rec[i, j], j);
                    Grid.SetRow(Rec[i, j], i);

                    GamePane.Children.Add(Rec[i, j]);

                    Rec[i, j].MouseLeftButtonUp += Rec_MouseUp;
                    Rec[i, j].MouseRightButtonDown += Rec_Back;

                    Rec[i, j].MouseEnter += Rec_Hover;
                }
            }
            // /Поле //
            // /Заполнение поля //

            // Очистка поля //
            whileBlock();
            // /Очистка поля //
            StartGame = true;

            SClass.IdSpellHer(IdHero);


            BotBrain.SpellMonster(MonsterNum);

            Lock.Visibility = Visibility.Hidden;
            Lock.Background = new ImageBrush(new BitmapImage(new Uri("images/Castle.png", UriKind.Relative)));

            Lock1.Visibility = Visibility.Hidden;
            Lock1.Background = new ImageBrush(new BitmapImage(new Uri("images/Castle.png", UriKind.Relative)));

            Lock2.Visibility = Visibility.Hidden;
            Lock2.Background = new ImageBrush(new BitmapImage(new Uri("images/Castle.png", UriKind.Relative)));

            Lock3.Visibility = Visibility.Hidden;
            Lock3.Background = new ImageBrush(new BitmapImage(new Uri("images/Castle.png", UriKind.Relative)));

            Lock4.Visibility = Visibility.Hidden;
            Lock4.Background = new ImageBrush(new BitmapImage(new Uri("images/Castle.png", UriKind.Relative)));

            JustSetToolClk();
            SClass.ValueL(true);
            JustCheckMana();
        }

        /// <summary>
        /// Когда приложение будет закрыто
        /// </summary>
        /// <param name="sender">Окно</param>
        /// <param name="e">Выход</param>
        private void Programm_Exit(object sender, ExitEventArgs e)
        {          
            if (myThread != null)
                myThread.Abort();     
        }

        /// <summary>
        /// Устанавливает рандомный фон игрового поля;
        /// </summary>
        private void SetBackgroundMainWindow()
        {
            MainW.Background = new ImageBrush(new BitmapImage(new Uri("images/Background-Panel/Pazzle" + randomGenerator.Next(1,4) + ".jpg", UriKind.Relative)));
        }

        //Устанавливает подсказку для способности;
        private void JustSetToolClk()
        {
            if (SpellСhoice[0] != -1)
                SClass.SetTool(NameSpell1, ClkSpell1, IdHero);

            if (SpellСhoice[1] != -1)
                SClass.SetTool(NameSpell2, ClkSpell2, IdHero);

            if (SpellСhoice[2] != -1)
                SClass.SetTool(NameSpell3, ClkSpell3, IdHero);

            if (SpellСhoice[3] != -1)
                SClass.SetTool(NameSpell4, ClkSpell4, IdHero);

            if (SpellСhoice[4] != -1)
                SClass.SetTool(NameSpell5, ClkSpell5, IdHero);
        }

        // Создание иконок баффов;
        private Label CreateBF()
        {
            Label lb = new Label();
            lb.Visibility = Visibility.Hidden;

            lb.Tag = "0";

            lb.Foreground = Brushes.White;
            return lb;
        }


        // Сравнивает кол-во вашей маны с маной которую надо затратить чтобы использовать спосбоность;
        public void CheckValueMana(Label GT, Label RT, Label YT, Label BT)
        {
            int g, r, y, b;

            g = Int32.Parse("" + greenL.Content);
            r = Int32.Parse("" + redL.Content);
            y = Int32.Parse("" + yellowL.Content);
            b = Int32.Parse("" + blueL.Content);

            try
            {
                if (g < Int32.Parse("" + GT.Content))
                    GT.Foreground = Brushes.Red;
                else
                    GT.Foreground = Brushes.Black;

                if (r < Int32.Parse("" + RT.Content))
                    RT.Foreground = Brushes.Red;
                else
                    RT.Foreground = Brushes.Black;

                if (y < Int32.Parse("" + YT.Content))
                    YT.Foreground = Brushes.Red;
                else
                    YT.Foreground = Brushes.Black;

                if (b < Int32.Parse("" + BT.Content))
                    BT.Foreground = Brushes.Red;
                else
                    BT.Foreground = Brushes.Black;
            }
            catch (FormatException) { }
        }

        // Указываем сколько будет стоить способность по мане, и, если стоимость равна 0, то убрать из списка;
        public void ScaleManaTop(int GreenСost, int RedCost, int YellowCost, int BlueCost,
                                  Label ManaGreen, Label ManaRed, Label ManaYellow, Label ManaBlue,
                                  Label TextGreen, Label TextRed, Label TextYellow, Label TextBlue)
        {

            if (GreenСost == 0)
            {
                ManaGreen.Visibility = Visibility.Hidden;
                TextGreen.Visibility = Visibility.Hidden;
            }
            if (RedCost == 0)
            {
                ManaRed.Visibility = Visibility.Hidden;
                TextRed.Visibility = Visibility.Hidden;
            }
            if (YellowCost == 0)
            {
                ManaYellow.Visibility = Visibility.Hidden;
                TextYellow.Visibility = Visibility.Hidden;
            }
            if (BlueCost == 0)
            {
                ManaBlue.Visibility = Visibility.Hidden;
                TextBlue.Visibility = Visibility.Hidden;
            }

            TextGreen.Content = "" + GreenСost;
            TextRed.Content = "" + RedCost;
            TextYellow.Content = "" + YellowCost;
            TextBlue.Content = "" + BlueCost;

        }


        //Поиск стаков которые нужно удалить (по ряду);
        private void ClearStackRow()
        {

            Rowint = 0;
            NumMassRow = 0;
            NoStackBlocks = true;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    try
                    {
                        if (Rec[i, j].Tag.Equals(Rec[i, j + 1].Tag) && Rec[i, j + 1].Tag.Equals(Rec[i, j + 2].Tag))
                        {
                            CheckToExtra = true;
                            NoStackBlocks = false;
                            if (j < 5)
                            {
                                if (Rec[i, j + 2].Tag.Equals(Rec[i, j + 3].Tag))
                                {
                                    ExtraTurn = true;
                                    if (j < 4)
                                    {
                                        if (Rec[i, j + 3].Tag.Equals(Rec[i, j + 4].Tag))
                                        {
                                            for (int dd = j; dd < j + 5; dd++)
                                            {
                                                RowRec[NumMassRow] = Rec[i, dd];
                                                NumMassRow++;
                                            }

                                            RecRowSG[Rowint] = 5;
                                            j += 2;
                                            Rowint++;
                                            // 5 Одинаковых блоков;
                                        }
                                        else
                                        {
                                            for (int dd = j; dd < j + 4; dd++)
                                            {
                                                RowRec[NumMassRow] = Rec[i, dd];
                                                NumMassRow++;
                                            }
                                            j++;
                                            RecRowSG[Rowint] = 4;
                                            Rowint++;
                                        }
                                        //4 Одинаковых блоков;
                                    }
                                    else
                                    {
                                        for (int dd = j; dd < j + 4; dd++)
                                        {
                                            RowRec[NumMassRow] = Rec[i, dd];
                                            NumMassRow++;
                                        }

                                        RecRowSG[Rowint] = 4;
                                        j++;
                                        Rowint++;
                                    }
                                    //4 Одинаковых блоков;
                                }
                                else
                                {
                                    for (int dd = j; dd < j + 3; dd++)
                                    {
                                        RowRec[NumMassRow] = Rec[i, dd];
                                        NumMassRow++;
                                    }

                                    RecRowSG[Rowint] = 3;
                                    Rowint++;

                                }   //3 Одинаковых блоков;
                            }
                            else
                            {
                                for (int dd = j; dd < j + 3; dd++)
                                {
                                    RowRec[NumMassRow] = Rec[i, dd];
                                    NumMassRow++;
                                }

                                RecRowSG[Rowint] = 3;
                                Rowint++;

                            }   //3 Одинаковых блоков;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    { }
                }
            }
            ClearStackColumm();
        }

        //Поиск стаков которые нужно удалить (по столбцу);
        private void ClearStackColumm()
        {
            NumMassColumm = 0;
            Colummint = 0;
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i <= 5; i++)
                {
                    try
                    {
                        if (Rec[i, j].Tag.Equals(Rec[i + 1, j].Tag) && Rec[i + 1, j].Tag.Equals(Rec[i + 2, j].Tag))
                        {
                            NoStackBlocks = false;
                            CheckToExtra = true;
                            if (i < 5)
                            {
                                if (Rec[i + 2, j].Tag.Equals(Rec[i + 3, j].Tag))
                                {
                                    ExtraTurn = true;
                                    if (i < 4)
                                    {
                                        if (Rec[i + 3, j].Tag.Equals(Rec[i + 4, j].Tag))
                                        {
                                            for (int dd = i; dd < i + 5; dd++)
                                            {
                                                ColummRec[NumMassColumm] = Rec[dd, j];
                                                NumMassColumm++;
                                            }

                                            RecColummSG[Colummint] = 5;
                                            i += 2;
                                            Colummint++;
                                            // 5 Одинаковых блоков;
                                        }
                                        else
                                        {
                                            for (int dd = i; dd < i + 4; dd++)
                                            {
                                                ColummRec[NumMassColumm] = Rec[dd, j];
                                                NumMassColumm++;
                                            }
                                            RecColummSG[Colummint] = 4;
                                            i++;
                                            Colummint++;
                                        }
                                        //4 Одинаковых блоков;
                                    }
                                    else
                                    {
                                        for (int dd = i; dd < i + 4; dd++)
                                        {
                                            ColummRec[NumMassColumm] = Rec[dd, j];
                                            NumMassColumm++;
                                        }
                                        RecColummSG[Colummint] = 4;
                                        i++;
                                        Colummint++;
                                    }
                                    //4 Одинаковых блоков;
                                }
                                else
                                {
                                    for (int dd = i; dd < i + 3; dd++)
                                    {
                                        ColummRec[NumMassColumm] = Rec[dd, j];
                                        NumMassColumm++;
                                    }
                                    RecColummSG[Colummint] = 3;

                                    Colummint++;

                                }   //3 Одинаковых блоков;
                            }
                            else
                            {
                                for (int dd = i; dd < i + 3; dd++)
                                {
                                    ColummRec[NumMassColumm] = Rec[dd, j];
                                    NumMassColumm++;
                                }


                                RecColummSG[Colummint] = 3;
                                Colummint++;

                            }   //3 Одинаковых блоков;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    { }
                }
            }

            ClearBlock();
        }

        //Удаление найденных стаков;
        private void ClearBlock()
        {

            int tempRow = 0;
            int tempCol = 0;
            int nulls = 0;
            for (int i = 0; i < Colummint; i++)
            {
                for (int j = 0; j < RecColummSG[i]; j++)
                {
                    ColummRec[j + tempCol].Fill = null;
                }
                Brain(RecColummSG[i], ColummRec[tempCol], StartGame);
                tempCol += RecColummSG[i];
            }
            for (int i = 0; i < Rowint; i++)
            {
                for (int j = 0; j < RecRowSG[i]; j++)
                {
                    if (RowRec[j + tempRow].Fill == null)
                        nulls++;
                    RowRec[j + tempRow].Fill = null;

                }
                Brain(RecRowSG[i] - nulls, RowRec[tempRow], StartGame);
                nulls = 0;
                tempRow += RecRowSG[i];
            }
        }

        //Заполнение удаленных объектов (Без анимации);
        public void MoveBlockAuto()
        {
            Rectangle newRec = new Rectangle();
            int HzItem = 1;
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Rec[i, j].Fill == null)
                    {
                    Hz:
                        if (i - HzItem >= 0)
                        {
                            if (Rec[i - HzItem, j].Fill != null)
                            {

                                //Меняем местами теги;
                                newRec.Tag = Rec[i - HzItem, j].Tag;
                                Rec[i - HzItem, j].Tag = Rec[i, j].Tag;
                                Rec[i, j].Tag = newRec.Tag;

                                //Меняем цвета блоков;
                                newRec.Fill = Rec[i - HzItem, j].Fill;
                                Rec[i - HzItem, j].Fill = Rec[i, j].Fill;
                                Rec[i, j].Fill = newRec.Fill;
                                HzItem = 1;

                                //    MessageBox.Show("1 " + Rec[i, j].Fill + "   " + Rec[i, j].Tag);

                            }
                            else
                            {
                                HzItem++;
                                goto Hz;
                            }
                        }
                        else
                        {
                            ZiroAddBlock();
                            HzItem = 1;
                            i--;
                            goto Hz;
                        }
                    }
                }
                ZiroAddBlock();

            }
        }

        // Добавление рандомного блока (место пустого блока);
        private  void ZiroAddBlock()
        {
            for (int j = 0; j < 8; j++)
            {
                if (Rec[0, j].Fill == null)
                {
                                  
                    Rec[0, j].Fill = Brushes_Rand(rnd.Next(1, 8), Rec[0, j]);
                    //       MessageBox.Show("1 " + Rec[0, j].Fill + "   " + Rec[0,j].Tag);
                }
            }

        }

        //МиниБрейн :) ;
        public void Brain(int NumPlus, Rectangle block, Boolean StartGame)
        {
            if (StartGame)
            {
                int n;
                try
                {
                    n = (int)block.Tag;
                    //   MessageBox.Show("" + n);

                    //Если мана, то + к общей мане;
                    if (n < 5)
                        Mana4(NumPlus, block, TurnHeroAndEnemy);

                    if (n == 5)
                        Damage(NumPlus, TurnHeroAndEnemy);

                    if (n > 5 && n < 8)
                        ExpGld(n, NumPlus, TurnHeroAndEnemy);



                    if (NumPlus >= 4)
                    {
                        DopEventTurn(NumPlus, n, block);
                    }
                    else
                        messageManaValue(NumPlus, n, block);
                }
                catch (NullReferenceException)
                { }
                SClass.ValueL(TurnHeroAndEnemy);

            }

            //   if (TurnHeroAndEnemy)

            JustCheckMana();
            JustCheckManaBot();

        }


        //Отображает количество разрушенных камней на поле, в виде цифровых значений;
        public void messageManaValue(int Num, int n, Rectangle block)
        {

            TextBlock tB = new TextBlock();

            tB.FontFamily = new FontFamily("Jokerman");
            tB.Foreground = SetForeg(n);
            // tB.Background = Brushes.White;

            double PointX = 0;

            tB.TextAlignment = TextAlignment.Center;
            tB.Width = 37;
            tB.Height = 45;

            Grid.SetColumn(tB, Grid.GetColumn(block));
            Grid.SetRow(tB, Grid.GetRow(block));


            tB.FontSize = 30;
            tB.Text = "+" + Num;

            GamePane.Children.Add(tB);

            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);

            timer.Tick += (s, o) =>
            {
                tB.Opacity -= 0.03;
                tB.Margin = new Thickness(block.Margin.Left, PointX, block.Margin.Right, block.Margin.Bottom);
                PointX++;
                if (tB.Opacity <= 0)
                {
                    GamePane.Children.Remove(tB);
                    PointX = 0;
                    timer.Stop();
                }
            };
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Start();

        }


        private Brush SetForeg(int n)
        {
            switch (n)
            {
                case 1:
                    return Brushes.Red;
                case 2:
                    return Brushes.Yellow;
                case 3:
                    return Brushes.Green;
                case 4:
                    return Brushes.Blue;
                case 5:
                    return Brushes.Black;
                case 6:
                    return Brushes.Indigo;
                case 7:
                    return Brushes.Gold;
                default:
                    return null;
            }
        }

        //Бесполезный метод (нет) 
        public void JustCheckMana()
        {
            CheckValueMana(GreenManaText, RedManaText, YellowManaText, BlueManaText);
            CheckValueMana(GreenManaText1, RedManaText1, YellowManaText1, BlueManaText1);
            CheckValueMana(GreenManaText2, RedManaText2, YellowManaText2, BlueManaText2);
            CheckValueMana(GreenManaText3, RedManaText3, YellowManaText3, BlueManaText3);
            CheckValueMana(GreenManaText4, RedManaText4, YellowManaText4, BlueManaText4);
        }

        //Бесполезный метод (нет) 2
        public void JustCheckManaBot()
        {
            BotBrain.CheckValueMana(GreenManaText5, RedManaText5, YellowManaText5, BlueManaText5);
            BotBrain.CheckValueMana(GreenManaText6, RedManaText6, YellowManaText6, BlueManaText6);
            BotBrain.CheckValueMana(GreenManaText7, RedManaText7, YellowManaText7, BlueManaText7);
            BotBrain.CheckValueMana(GreenManaText8, RedManaText8, YellowManaText8, BlueManaText8);
            BotBrain.CheckValueMana(GreenManaText9, RedManaText9, YellowManaText9, BlueManaText9);
        }

        //Определение золото или опыт и + к общему значению (золота или опыта);
        public void ExpGld(int n, int NumN, Boolean Turn)
        {
            if (n == 6)
            {
                if (Turn)
                {
                    if (SClass.Trade > 0)
                    {
                        for (int i = 0; i < NumN; i++)
                            heroP.Value += SClass.TradePal();

                        heroL.Content = heroP.Value;
                    }
                    expL.Content = (Int32.Parse(Convert.ToString(expL.Content)) + NumN);
                }
                else
                    expLE.Content = (Int32.Parse(Convert.ToString(expLE.Content)) + NumN);
            }
            else
            {
                if (Turn)
                    goldL.Content = (Int32.Parse(Convert.ToString(goldL.Content)) + NumN);
                else
                    goldLE.Content = (Int32.Parse(Convert.ToString(goldLE.Content)) + NumN);

            }
        }

        /// <summary>
        /// Отображение кол-во здоровья
        /// </summary>
        /// <param name="NumN">Количество ущерба</param>
        /// <param name="Turn">Кому будет нанесён урон</param>
        public void Damage(int NumN, Boolean Turn)
        {
            if (SClass.FightPal > 0)
            {
                NumN += (int)NumN / 2;
            }

            if (Turn)
            {
                enemyP.Value = enemyP.Value - (NumN + SClass.HandPower[0]);
                enemyL.Content = enemyP.Value;
                SClass.blindness = 0;

                //Провиряем поющие мечи(War);
                SwordTurn();
                ///////////

                if (SClass.BlindTF[0])
                {
                    for (int i = 6; i >= 6; i--)
                    {
                        if (BFEnemy[i].Tag.Equals(4))
                        {
                            BFEnemy[i].Content = "";
                            BFEnemy[i].Tag = "0";
                            BFEnemy[i].Visibility = Visibility.Hidden;
                        }
                    }
                    //Двигаем баффы врага;
                    SClass.MoveBuffsEn();
                }
            }
            else
            {
                if (SClass.TryDamage)
                {
                    heroP.Value = heroP.Value - (NumN - SClass.ShealdSpell[0]);
                    heroL.Content = heroP.Value;
                }
                else
                {
                    // Маг (Стена огня)
                    if (SClass.WallFire)
                    {
                        Walls(NumN, redP, redL, SClass.WallFire, 5);
                    }

                        //Друид (Стена шипов)
                    else if (SClass.WallSpike)
                    {
                        Walls(NumN, greenP, greenL, SClass.WallSpike, 8);
                    }
                }
            }
        }

        private void Walls(int NumN, ProgressBar rg, Label rlgl, Boolean Sp, int NumberWall)
        {
            if (rg.Value - NumN > 1)
            {
                rg.Value -= NumN - SClass.ShealdSpell[0];
                rlgl.Content = rg.Value - 1;
            }
            else
            {
                SClass.TryDamage = true;
                for (int i = 0; i < 7; i++)
                {
                    if (BFHer[i].Tag.Equals(NumberWall))
                    {
                        BFHer[i].Tag = "0";
                        break;
                    }
                }
                Sp = false;

                SClass.MoveBuffs();
                Damage(NumN, TurnHeroAndEnemy);

            }
        }

        //Поющие мечи
        private void SwordTurn()
        {
            if (SClass.SwordsTurn != 0)
            {
                if (greenPE.Value - 4 <= 0)
                    greenPE.Value = 1;

                else
                    greenPE.Value -= 4;

                if (redPE.Value - 4 <= 0)
                    redPE.Value = 1;

                else
                    redPE.Value -= 4;

                if (yellowPE.Value - 4 <= 0)
                    yellowPE.Value = 1;

                else
                    yellowPE.Value -= 4;

                if (bluePE.Value - 4 <= 0)
                    bluePE.Value = 1;

                else
                    bluePE.Value -= 4;

                SClass.ValueL(false);

            }
        }

        //Определяем цвет маны;
        public void Mana4(int NumPlus, Rectangle block, Boolean Turn)
        {
            int n = (int)block.Tag;

            switch (n)
            {
                case 1:
                    if (Turn)
                        redP.Value = redP.Value + (1 * NumPlus);
                    else
                        redPE.Value = redPE.Value + (1 * NumPlus);
                    break;
                case 2:
                    if (Turn)
                        yellowP.Value = yellowP.Value + (1 * NumPlus);
                    else
                        yellowPE.Value = yellowPE.Value + (1 * NumPlus);
                    break;
                case 3:
                    if (Turn)
                        greenP.Value = greenP.Value + (1 * NumPlus);
                    else
                        greenPE.Value = greenPE.Value + (1 * NumPlus);
                    break;
                case 4:
                    if (Turn)
                        blueP.Value = blueP.Value + (1 * NumPlus);
                    else
                        bluePE.Value = bluePE.Value + (1 * NumPlus);
                    break;
            }
        }

        //Отмена нажатия на ПКМ;
        private void Rec_Back(object sender, MouseButtonEventArgs e)
        {
            CheckMouseUp = false;
            SClass.CheckSquare = false;
            SClass.CheckKrest = false;
            SClass.CircularAttack = false;
            SClass.ColummDestroid = false;
            SClass.GrozaDestro = false;
            SClass.StoneDes = false;
            SClass.UpPal = false;
            SClass.RowDestroid = false;

            OpaF5();
        }

        //Нажатие на блок ЛКМ;
        private void Rec_MouseUp(object sender, MouseButtonEventArgs e)
        {

            var rec = sender as Rectangle;

            BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec)].Visibility = Visibility.Visible;


            if (!SClass.CheckSquare && !SClass.CheckKrest && !SClass.CircularAttack && !SClass.ColummDestroid &&
                !SClass.GrozaDestro && !SClass.StoneDes && !SClass.UpPal && !SClass.RowDestroid)
                MoveBlock(rec);

            else
            {
                if (SClass.CheckSquare)
                    SClass.Squares(rec);

                else if (SClass.CheckKrest)
                    SClass.Krest(rec);

                else if (SClass.CircularAttack)
                    SClass.Circular(rec);

                else if (SClass.ColummDestroid)
                    SClass.ColummsAndValue(rec);

                else if (SClass.GrozaDestro)
                    SClass.GrozaDestroid(rec);

                else if (SClass.StoneDes)
                    SClass.StoneDestroid(rec);

                else if (!rec.Tag.Equals(6) && SClass.UpPal)
                    SClass.UpKnight((int)rec.Tag);

                else if (SClass.RowDestroid)
                    SClass.RowDestr(rec);
            }
        }

        //Перемещение блоков;
        private void MoveBlock(Rectangle Block)
        {
            if (CheckMouseUp)
            {
                if (Grid.GetRow(SGRec) == Grid.GetRow(Block) && (Grid.GetColumn(SGRec) == Grid.GetColumn(Block) + 1 || Grid.GetColumn(SGRec) == Grid.GetColumn(Block) - 1) ||
                    Grid.GetColumn(SGRec) == Grid.GetColumn(Block) && (Grid.GetRow(SGRec) == Grid.GetRow(Block) + 1 || Grid.GetRow(SGRec) == Grid.GetRow(Block) - 1))
                {
                    //  Rectangle rc = new Rectangle();

                    OpaF5();
                    Counter counter = new Counter();

                    Point position = SGRec.TranslatePoint(new Point(0, 0), (VisualTreeHelper.GetParent(MainW) as UIElement));

                    Point position2 = Block.TranslatePoint(new Point(0, 0), (VisualTreeHelper.GetParent(MainW) as UIElement));


                    counter.Recs1 = SGRec;
                    counter.Recs2 = Block;
                    counter.trueRepeat = false;
                    counter.botTurn = false;
                    counter.p1 = position;
                    counter.p2 = position2;

                    myThread = new Thread(new ParameterizedThreadStart(JustMoveBlock));
                    myThread.SetApartmentState(ApartmentState.STA);
                    myThread.Start(counter);

                    MainW.IsEnabled = false;
                }

                else
                {
                    BackgroundRecHover[Grid.GetRow(SGRec), Grid.GetColumn(SGRec)].Visibility = Visibility.Hidden;
                    SGRec = Block;

                    BackgroundRecHover[Grid.GetRow(Block), Grid.GetColumn(Block)].Visibility = Visibility.Visible;
                }
            }
            else
            {

                SGRec = Block;
                CheckMouseUp = true;
            }
        }


        // Создание Hover блоков;
        private Rectangle CreateItemBack()
        {
            Rectangle NRec = new Rectangle();


            NRec.Fill = Brushes.CornflowerBlue;

            NRec.Visibility = Visibility.Hidden;
            return NRec;
        }

        //Создание блоков;
        private Rectangle CreateItem()
        {
            Rectangle NRec = new Rectangle();
            NRec.Fill = Brushes_Rand(rnd.Next(1, 8), NRec);
            NRec.Stretch = Stretch.Uniform;
            return NRec;
        }

        //Рандом цвета блоков;
        public ImageBrush Brushes_Rand(int temp, Rectangle Recs)
        {
            ImageBrush brush = new ImageBrush();

            Recs.Tag = temp;

            switch (temp)
            {
                case 1:
                    brush.ImageSource = new BitmapImage(new Uri("images/RedMana.png", UriKind.Relative));
                    break;
                case 2:

                    brush.ImageSource = new BitmapImage(new Uri("images/YellowMana.png", UriKind.Relative));

                    break;
                case 3:

                    brush.ImageSource = new BitmapImage(new Uri("images/GreenMana.png", UriKind.Relative));
                    break;
                case 4:

                    brush.ImageSource = new BitmapImage(new Uri("images/BlueMana.png", UriKind.Relative));
                    break;
                case 5:

                    brush.ImageSource = new BitmapImage(new Uri("images/Skull.png", UriKind.Relative));
                    break;
                case 6:

                    brush.ImageSource = new BitmapImage(new Uri("images/Exp.png", UriKind.Relative));
                    break;
                case 7:

                    brush.ImageSource = new BitmapImage(new Uri("images/Gold.png", UriKind.Relative));
                    break;
            }
            return brush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MetodCheckBlock();

        }

        // Смена хода;
        public Boolean SwapBool(Boolean Bool)
        {

            if (heroP.Value == 0 || enemyP.Value == 0)
            {
                if (heroP.Value == 0)
                {
                    MessageBox.Show("Пффф, ну ты и лох. Ты проиграл. :( ");
                }
                else if (enemyP.Value == 0)
                {
                    MessageBox.Show("Ты победил, ееее бой");
                }

                MenuBack = new MainMenu();
                MenuBack.Show();
                Close();

                return false;
            }
            else
            {
                if (SClass.AyraPal > 0)
                {
                    greenP.Value += 2;
                    redP.Value += 2;
                    blueP.Value += 2;
                    yellowP.Value += 2;

                    SClass.ValueL(true);
                }

                FullClear();
                if (Bool)
                {
                    //Уменьшаем длительность баффов на враге;
                    SClass.BuffsMunusTimeEnemy();

                    Bool = true;
                    if (SClass.blindness > 0 || SClass.Entang > 0)
                    {
                        if (SClass.blindness > 0)
                            SClass.blindness--;
                        if (SClass.Entang > 0)
                            SClass.Entang--;

                        MessageBox.Show("Свет жжет глаза!");

                        MainW.IsEnabled = true;
                    }
                    else
                    {

                        TurnEnemy.Background = Brushes.Green;
                        TurnHer.Background = Brushes.Red;

                        BotTurns();
                    }

                    JustCheckMana();
                    JustCheckManaBot();
                    return Bool;
                }
                else
                {
                    if (SClass.ShealdSpell[1] > 0)
                    {
                        SClass.ShealdSpell[0] = 1;
                        SClass.ShealdSpell[1]--;
                    }
                    else
                        SClass.ShealdSpell[0] = 0;

                    if (SClass.Speshka[1] > 0)
                        SClass.Speshka[1]--;

                    if (SClass.HandPower[1] > 0)
                        SClass.HandPower[1]--;
                    else
                        SClass.HandPower[0] = 0;

                    if (SClass.FightPal > 0)
                        SClass.FightPal--;

                    if (SClass.Trade > 0)
                        SClass.Trade--;

                    if (SClass.Insomnia > 0)
                        SClass.Insomnia--;

                    if (SClass.AyraPal > 0)
                        SClass.AyraPal--;

                    SClass.BuffsMinusTime();

                    SClass.OffLock(ClkSpell1, Lock);
                    SClass.OffLock(ClkSpell2, Lock1);
                    SClass.OffLock(ClkSpell3, Lock2);
                    SClass.OffLock(ClkSpell4, Lock3);
                    SClass.OffLock(ClkSpell5, Lock4);

                    Bool = true;
                    TurnEnemy.Background = Brushes.Red;
                    TurnHer.Background = Brushes.Green;

                    JustCheckMana();
                    FixTroct();

                    return Bool;
                }
            }
        }

        //Исправляет баги с анимцией в случае чего;
        private void FixTroct()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Rec[i, j].Fill == Brushes.Transparent)
                        Brushes_Rand(Int32.Parse("" + Rec[i, j].Tag), Rec[i, j]);
                }
            }
        }

        // Запуск класса BrainBot который выполняет обработку хода бота;
        public void BotTurns()
        {

            TurnHeroAndEnemy = false;
            NoStackBlocks = false;
            ExtraTurn = false;
        //    DispatcherTimer t = new DispatcherTimer();
          //  t.Interval = TimeSpan.FromSeconds(0.5);
        //    t.Tick += (s, e) =>
         //       {
         //           if (bl)
         //           {
                        BotBrain.BotTurn(false);
          //              t.Stop();
         //           }
        //        };
         //   t.Start();
        }

        // <Click Spell> //

        private void ClkSpell1_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            SClass.CheckValueManaZatrat(GreenManaText, RedManaText, YellowManaText, BlueManaText, SpellСhoice[0], bt);
        }

        private void ClkSpell2_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            SClass.CheckValueManaZatrat(GreenManaText1, RedManaText1, YellowManaText1, BlueManaText1, SpellСhoice[1], bt);
        }

        private void ClkSpell3_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            SClass.CheckValueManaZatrat(GreenManaText2, RedManaText2, YellowManaText2, BlueManaText2, SpellСhoice[2], bt);
        }

        private void ClkSpell4_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            SClass.CheckValueManaZatrat(GreenManaText3, RedManaText3, YellowManaText3, BlueManaText3, SpellСhoice[3], bt);
        }

        private void ClkSpell5_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            SClass.CheckValueManaZatrat(GreenManaText4, RedManaText4, YellowManaText4, BlueManaText4, SpellСhoice[4], bt);
        }

        // </Click Spell> //


        // Обновления поля;
        public void OpaF5()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BackgroundRecHover[i, j].Visibility = Visibility.Hidden;
                }
            }
        }

        // Если взят в таргет спелл который бьет по области, то подсвечивать область;
        private void Rec_Hover(object sender, MouseEventArgs e)
        {

            var rec = sender as Rectangle;
            if (SClass.CheckSquare)
            {
                OpaF5();

                if (Grid.GetRow(rec) - 1 >= 0)
                {
                    if (Grid.GetColumn(rec) - 1 >= 0)
                        try { BackgroundRecHover[Grid.GetRow(rec) - 1, Grid.GetColumn(rec) - 1].Visibility = Visibility.Visible; }
                        catch (IndexOutOfRangeException) { }

                    if (Grid.GetColumn(rec) + 1 < 8)
                        try { BackgroundRecHover[Grid.GetRow(rec) - 1, Grid.GetColumn(rec) + 1].Visibility = Visibility.Visible; }
                        catch (IndexOutOfRangeException) { }

                    try { BackgroundRecHover[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                    catch (IndexOutOfRangeException) { }
                }

                if (Grid.GetColumn(rec) - 1 >= 0)
                {

                    if (Grid.GetRow(rec) + 1 < 8)
                        try { BackgroundRecHover[Grid.GetRow(rec) + 1, Grid.GetColumn(rec) - 1].Visibility = Visibility.Visible; }
                        catch (IndexOutOfRangeException) { }

                    try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) - 1].Visibility = Visibility.Visible; }
                    catch (IndexOutOfRangeException) { }
                }


                if (Grid.GetColumn(rec) + 1 < 8)
                    try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) + 1].Visibility = Visibility.Visible; }
                    catch (IndexOutOfRangeException) { }

                if (Grid.GetRow(rec) + 1 < 8)
                {
                    if (Grid.GetColumn(rec) + 1 < 8)
                        try { BackgroundRecHover[Grid.GetRow(rec) + 1, Grid.GetColumn(rec) + 1].Visibility = Visibility.Visible; }
                        catch (IndexOutOfRangeException) { }

                    try { BackgroundRecHover[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                    catch (IndexOutOfRangeException) { }
                }

                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }

            }
            else if (SClass.CheckKrest)
            {
                OpaF5();
                try { BackgroundRecHover[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec) - 2, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) - 1].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) - 2].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec) + 2, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) + 1].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) + 2].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
            }
            else if (SClass.CircularAttack)
            {
                OpaF5();

                try { BackgroundRecHover[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) + 1].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }
                try { BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec) - 1].Visibility = Visibility.Visible; }
                catch (IndexOutOfRangeException) { }

            }

            else if (SClass.ColummDestroid || SClass.GrozaDestro)
            {
                OpaF5();

                for (int i = 0; i < 8; i++)
                {
                    BackgroundRecHover[i, Grid.GetColumn(rec)].Visibility = Visibility.Visible;
                }
            }

            else if (SClass.StoneDes || SClass.UpPal)
            {
                OpaF5();
                BackgroundRecHover[Grid.GetRow(rec), Grid.GetColumn(rec)].Visibility = Visibility.Visible;
            }

            else if (SClass.RowDestroid)
            {
                OpaF5();

                for (int i = 0; i < 8; i++)
                {
                    BackgroundRecHover[Grid.GetRow(rec), i].Visibility = Visibility.Visible;
                }
            }
        }

        //Событие хода;
        public void MetodCheckBlock()
        {
            CheckToExtra = false;
            NoStackBlocks = false;
            ExtraTurn = false;

            whileBlock();

            if (CheckToExtra && !ExtraTurn || SClass.CheckSquare && !ExtraTurn || SClass.CheckKrest && !ExtraTurn ||
                SClass.CircularAttack && !ExtraTurn || SClass.ColummDestroid && !ExtraTurn || SClass.GrozaDestro && !ExtraTurn ||
                SClass.StoneDes && !ExtraTurn || SClass.UpPal && !ExtraTurn || SClass.RowDestroid && !ExtraTurn)
            {
                //   MessageBox.Show("ХОД 222 " + TurnHeroAndEnemy);
                TurnHeroAndEnemy = SwapBool(TurnHeroAndEnemy);
            }


            else if (ExtraTurn && TurnHeroAndEnemy && SClass.Speshka[1] != 0)
            {
                Damage(4, true);
            }

            else if (ExtraTurn && TurnHeroAndEnemy && SClass.Insomnia != 0)
            {
                greenP.Value += 3;
                redP.Value += 3;
                yellowP.Value += 3;
                blueP.Value += 3;

                SClass.ValueL(true);
            }

            SClass.CheckSquare = false;
            SClass.CheckKrest = false;
            SClass.CircularAttack = false;
            SClass.ColummDestroid = false;
            SClass.GrozaDestro = false;
            SClass.StoneDes = false;
            SClass.UpPal = false;
            SClass.RowDestroid = false;
        }

        //Очистка поля после того, как было обнаружено отсутствие возможных ходов;
        public void FullClear()
        {
            if (BotBrain.NoTurns())
            {
                MessageBox.Show("СТОЙ");
                MessageBox.Show("СТОЙ");
                MessageBox.Show("СТОЙ");

                MessageBox.Show("СТОЙ");
                MessageBox.Show("СТОЙ");

                MessageBox.Show("СТОЙ");
                MessageBox.Show("СТОЙ");
                MessageBox.Show("СТОЙ");
                //Стена огня(минус);
                for (int i = 0; i < 7; i++)
                {
                    if (BFHer[i].Tag.Equals(5))
                    {
                        BFHer[i].Tag = "0";
                    }
                }

                SClass.MoveBuffs();


                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Rec[i, j].Fill = null;
                    }
                }



                StartGame = false;

                whileBlock();
                StartGame = true;

                greenP.Value = 1;
                redP.Value = 1;
                yellowP.Value = 1;
                blueP.Value = 1;

                greenPE.Value = 1;
                redPE.Value = 1;
                yellowPE.Value = 1;
                bluePE.Value = 1;

                SClass.ValueL(true);
                SClass.ValueL(false);

                JustCheckMana();
                JustCheckManaBot();
            }
        }

        // Чистим стаки пока они не закончатся;
        public void whileBlock()
        {
            NoStackBlocks = false;
           // bl = false;
            while (!NoStackBlocks)
            {          
                MoveBlockAuto();
                MoveBlockAuto();
                              
                ClearStackRow();              
           }         
        }

        //$$ Brain Animations $$//

        public void JustMoveBlock(object obj)
        {
            Counter c = (Counter)obj;

            lightAnimation(c.Recs1, c.Recs2, c.trueRepeat, c.botTurn, c.p1, c.p2);

        }


        //Простая анимация передвижения блоков;
        public void lightAnimation(Rectangle block1, Rectangle block2, Boolean TrRepeat, Boolean BotTurn, Point position, Point position2)
        {
            Semaphore sem = new Semaphore(1, 2);

            sem.WaitOne();

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                Rectangle b1 = new Rectangle();

                Rectangle b2 = new Rectangle();


                if (BotTurn)
                    TurnHeroAndEnemy = false;

                b1.Width = block1.ActualWidth + 7;
                b1.Height = block1.ActualHeight + 7;

                b2.Width = block2.ActualWidth + 7;
                b2.Height = block2.ActualHeight + 7;

                b1.Fill = block1.Fill;
                b2.Fill = block2.Fill;

                b1.Tag = block1.Tag;
                b2.Tag = block2.Tag;

                block1.Fill = Brushes.Transparent;
                block2.Fill = Brushes.Transparent;

                Canvas.SetLeft(b1, position.X);
                Canvas.SetTop(b1, position.Y);

                Canvas.SetLeft(b2, position2.X);
                Canvas.SetTop(b2, position2.Y);


                MainW.Children.Add(b1);
                MainW.Children.Add(b2);


                //////////////////////

                var anim1 = new ThicknessAnimation();

                anim1.From = new Thickness(0, 0, 0, 0);
                anim1.To = new Thickness(SetMarginLeft(block1, block2, true), SetMarginTop(block1, block2, true), 0, 0);

                //Время первой анимации;
                anim1.Duration = TimeSpan.FromSeconds(0.20);

                Storyboard.SetTarget(anim1, b1);
                Storyboard.SetTargetProperty(anim1, new PropertyPath(MarginProperty));

                var anim2 = new ThicknessAnimation();

                anim2.From = new Thickness(0, 0, 0, 0);
                anim2.To = new Thickness(SetMarginLeft(block1, block2, false), SetMarginTop(block1, block2, false), 0, 0);

                //Время второй анимации;
                anim2.Duration = TimeSpan.FromSeconds(0.20);

                Storyboard.SetTarget(anim2, b2);
                Storyboard.SetTargetProperty(anim2, new PropertyPath(MarginProperty));


                //////////////////////////////////////


                var storyboard = new Storyboard();



                storyboard.Children = new TimelineCollection { anim1, anim2 };

                storyboard.Completed += (s, e) =>
                {


                    if (!BotTurn)
                        CheckMouseUp = false;

                    block1.Fill = b2.Fill;
                    block2.Fill = b1.Fill;

                    block1.Tag = b2.Tag;
                    block2.Tag = b1.Tag;


                    MainW.Children.Remove(b1);
                    MainW.Children.Remove(b2);

                    if (TrRepeat && !BotTurn)
                    {
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.25) };
                        timer.Start();
                        timer.Tick += (sender, args) =>
                        {
                            timer.Stop();

                            TurnHeroAndEnemy = SwapBool(TurnHeroAndEnemy);


                        };
                    }

                    if (!TrRepeat)
                    {
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.15) };
                        timer.Start();
                        timer.Tick += (sender, args) =>
                        {
                            timer.Stop();

                            MetodCheckBlock();

                            //      Thread.Sleep(150);
                        };

                        var timer2 = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.2) };
                        timer2.Start();
                        timer2.Tick += (sender, args) =>
                        {
                            timer2.Stop();
                            if (ExtraTurn && BotTurn)
                            {
                                BotTurns();
                            }
                            else if (!ExtraTurn && BotTurn || ExtraTurn && !BotTurn)
                            {
                                MainW.IsEnabled = true;
                            }

                            //   Thread.Sleep(200);
                        };


                        if (!BotTurn)
                        {

                            var timer3 = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(0.25) };
                            timer3.Start();
                            timer3.Tick += (sender, args) =>
                            {
                                timer3.Stop();

                                if (!CheckToExtra && !BotTurn)
                                {
                                    lightAnimation(block1, block2, true, false, position, position2);
                                    MainW.IsEnabled = false;
                                    messageManaValue(5, 5, block1);
                                    Damage(5, false);
                                    storyboard.Stop();
                                }

                                //   Thread.Sleep(250);
                            };
                        }
                        OpaF5();
                    }
                    storyboard.Stop();
                };
                storyboard.Begin();
            }));
            sem.Release();
        }


        private double SetMarginTop(Rectangle block1, Rectangle block2, bool p)
        {
            double tt = 0;

            if (Grid.GetColumn(block1) == Grid.GetColumn(block2) && Grid.GetRow(block1) == Grid.GetRow(block2) - 1)
            {
                tt = 75;
                if (!p)
                    tt *= -1;
            }
            else if (Grid.GetColumn(block1) == Grid.GetColumn(block2) && Grid.GetRow(block1) - 1 == Grid.GetRow(block2))
            {
                tt = 75;
                if (p)
                    tt *= -1;
            }
            return tt;
        }

        private double SetMarginLeft(Rectangle block1, Rectangle block2, bool p)
        {
            double tt = 0;

            if (Grid.GetColumn(block1) - 1 == Grid.GetColumn(block2) && Grid.GetRow(block1) == Grid.GetRow(block2))
            {
                tt = 75;
                if (p)
                    tt *= -1;
            }
            else if (Grid.GetColumn(block1) == Grid.GetColumn(block2) - 1 && Grid.GetRow(block1) == Grid.GetRow(block2))
            {
                tt = 75;
                if (!p)
                    tt *= -1;
            }
            return tt;
        }

        public void DopEventTurn(int value, int ColorF, Rectangle PointXY)
        {
            Label DopTurnTb = new Label();

            // Анимация //
            var MoveTb = new ThicknessAnimation();

            var Opacivi = new DoubleAnimation();

            Opacivi.From = 1;
            Opacivi.To = 0;
            Opacivi.Duration = TimeSpan.FromSeconds(1.05);

            MoveTb.From = new Thickness(0, -5, 0, 0);
            MoveTb.To = new Thickness(0, 10, 0, 0);
            MoveTb.Duration = TimeSpan.FromSeconds(1);
            // /Анимация //


            DopTurnTb.HorizontalContentAlignment = HorizontalAlignment.Center;
            DopTurnTb.VerticalContentAlignment = VerticalAlignment.Center;
            DopTurnTb.Content = value.ToString() + " Одного вида " + "\nЕще один ход";

            DopTurnTb.BorderBrush = Brushes.YellowGreen;
            DopTurnTb.BorderThickness = new Thickness(1.5);

            DopTurnTb.Foreground = Brushes.Orange;

            DopTurnTb.FontFamily = new FontFamily("Times new Roman");
            DopTurnTb.FontSize = 32;

            Point position = PointXY.TranslatePoint(new Point(0, 0), (VisualTreeHelper.GetParent(MainW) as UIElement));

            Canvas.SetLeft(DopTurnTb, position.X);
            Canvas.SetTop(DopTurnTb, position.Y);

            MainW.Children.Add(DopTurnTb);

            MoveTb.Completed += (s, e) =>
                {
                    MainW.Children.Remove(DopTurnTb);
                };

            DopTurnTb.BeginAnimation(MarginProperty, MoveTb);
            DopTurnTb.BeginAnimation(OpacityProperty, Opacivi);
        }

        /// <summary>
        ///  Считывает игровое поле, для того чтобы определить есть ли дополнительный ход после 
        ///  использования способности, или нет. Меняет ход если не было найдено стаков 4 или 5. 
        /// </summary>
        /// <param name="turn">Принимает bool значение, чей ход сейчас будет провирятся.</param>
        public void CheckToDestroidBlock(Boolean turn)
        {
            ExtraTurn = false;
            MessageBox.Show("1");
            whileBlock();
            //Смена хода
            MessageBox.Show(ExtraTurn.ToString());
            if (!ExtraTurn)
                TurnHeroAndEnemy = SwapBool(TurnHeroAndEnemy);

            if(!turn)
            {
                BotTurns();
            }
        }

        /*
        public void AnimationBlockMove()
        {
            Thread AnimationMovesBlock = new Thread(new ThreadStart(MoveBlockAutoAnim));
            AnimationMovesBlock.SetApartmentState(ApartmentState.STA);
            AnimationMovesBlock.Start();
        }
        */

        //////////////!!!!!!!!!!!!!!Я ХЗ КАК ЭТО ДЕЛАТЬ, У МЕНЯ ЖОПА ГОРИТ, ПЛИЗ УБЕЙТЕ МЕНЯ!!!!!!!!!!!!!! :(((((((
        /*
        //Заполнение удаленных объектов (С анимацией);
        public async void MoveBlockAutoAnim()
        {
       //     int tempVal = 0;

            Rectangle newRec = new Rectangle();
            int HzItem = 1;
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    try
                    {
                        if (Rec[i, j].Fill == null)
                        {
                        Hz:
                            if (i - HzItem >= 0)
                            {
                                if (Rec[i - HzItem, j].Fill != null)
                                {

                                    //  MessageBox.Show(i.ToString());
                                    //  tempVal++;

                                    //Меняем местами теги;
                                    newRec.Tag = Rec[i - HzItem, j].Tag;
                                    Rec[i - HzItem, j].Tag = Rec[i, j].Tag;
                                    Rec[i, j].Tag = newRec.Tag;

                                    //Меняем цвета блоков;
                                    newRec.Fill = Rec[i - HzItem, j].Fill;
                                    Rec[i - HzItem, j].Fill = Rec[i, j].Fill;
                                    Rec[i, j].Fill = newRec.Fill;
                                    HzItem = 1;
                                    await Task.Delay(30);


                                    //    MessageBox.Show("1 " + Rec[i, j].Fill + "   " + Rec[i, j].Tag);

                                }
                                else
                                {

                                    //   Animation(i,tempVal, Rec[i - HzItem, j]);
                                    //   tempVal = 0;
                                    HzItem++;
                                    goto Hz;
                                }
                            }
                            else
                            {
                                // Animation(i, tempVal, Rec[0, j]);
                               
                                ZiroAddBlock();
                                HzItem = 1;
                                i--;
                                goto Hz;
                            }
                        }

                        ZiroAddBlock();
                    }
                    catch (IndexOutOfRangeException) { }
                }
                bl = true;
            }
        
        }
         * */
        /*
        private void Animation(int NumRow ,int tempVal, Rectangle rects)
        {
            Grid grid = new Grid();

            for(int i = 0; i < tempVal; i++)
            {
                RowDefinition a = new RowDefinition();
                grid.RowDefinitions.Add(a);
                Rectangle rs = new Rectangle();
                rs.Fill = Rec[NumRow - i, Grid.GetColumn(rects)].Fill;
                rs.Tag = Rec[NumRow - i, Grid.GetColumn(rects)].Tag;

                Grid.SetRow(rs, i);
                rememberBlockAnim[i] = rs;
                grid.Children.Add(rs);
            }

            grid.Width = rects.ActualWidth + 1;

            grid.Height = rects.ActualHeight * tempVal + 1;

            Point position = rects.TranslatePoint(new Point(0, 0), (VisualTreeHelper.GetParent(MainW) as UIElement));

            Canvas.SetLeft(grid, position.X);
            Canvas.SetTop(grid, position.Y);

            MoveGridBlock(grid,NumRow, tempVal, rects);

        }

        private void MoveGridBlock(Grid grid,int NumRow, int tempVal, Rectangle rects)
        {
            var anim = new ThicknessAnimation();

            MainW.Children.Add(grid);

            anim.From = new Thickness(0, 0, 0, 0);
            anim.To = new Thickness(0, 0, 0, grid.ActualHeight - 2);

            anim.Duration = TimeSpan.FromSeconds(0.5);

            anim.Completed += (s, e) =>
                {
                    ZiroAddBlock();
                };
            grid.BeginAnimation(MarginProperty, anim);

        }*/
    }

    public class Counter
    {
        public Rectangle Recs1;
        public Rectangle Recs2;
        public Boolean trueRepeat;
        public Boolean botTurn;
        public Point p1;
        public Point p2;
    }
}