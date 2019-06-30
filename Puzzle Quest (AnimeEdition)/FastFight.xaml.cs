using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Puzzle_Quest__AnimeEdition_
{
    /// <summary>
    /// Interaction logic for FastFight.xaml
    /// </summary>
    public partial class FastFight : Window
    {
        
        public int FordBack = 0; // id героя;
        public int FordBackE = 0; // id Монстра;

        //Скелет 40 хп;
        //Троль 110 хп;

        //Король Лич 1200 хп;
     

        public int maxEnemy = (3) - 1;

        int[] Spells = new int[5];

        CheckBox[] Ch = new CheckBox[14];

        int NumberTS = 0;
        public FastFight()
        {
            InitializeComponent();


            SpellsClass.Visibility = Visibility.Hidden;
            NameHeroes(0);
            NameEnemys(0);

          

            for(int i = 0; i < 5; i++)
            {
                Spells[i] = -1;
            }
        }

        private CheckBox CreateCh( int i)
        {
            CheckBox cb = new CheckBox();
            cb.Content = NameCls(i, FordBack);
            cb.Tag = i;
            cb.Checked += Ch_Checked;
            cb.Unchecked += Ch_Unchecked;
          
            return cb;
           
        }

        private void SetTool(int Id)
        {
            for(int i = 0; i < 14; i++)
            {
                TextBlock tb = new TextBlock();

                tb.Text = SpellClass.JustFindFileTool((string)Ch[i].Content, Id);
                tb.TextAlignment = TextAlignment.Center;

                Ch[i].ToolTip = tb;
            }
        }

        private void Marg(CheckBox cb)
        {
            cb.Margin = new Thickness(0, 5, 0, 0);
        }

        public static string NameCls(int i, int Cls)
        {
            String ss = null;
            if (Cls == 0)
            {
                switch (i)
                {
                        //Маг
                    case 0:
                        ss = "Огненная стрела";
                        return ss;
                    case 1:
                        ss = "Магический щит";
                        return ss;
                    case 2:
                        ss = "Спешка";
                        return ss;
                    case 3:
                        ss = "Огненный шар";
                        return ss;
                    case 4:
                        ss = "Огненный канал";
                        return ss;
                    case 5:
                        ss = "Рука силы";
                        return ss;
                    case 6:
                        ss = "Утечка тепла";
                        return ss;
                    case 7:
                        ss = "Стена огня";
                        return ss;
                    case 8:
                        ss = "Сжигание маны";
                        return ss;
                    case 9:
                        ss = "Свет";
                        return ss;
                    case 10:
                        ss = "Горящие черепа";
                        return ss;
                    case 11:
                        ss = "Прижигание";
                        return ss;
                    case 12:
                        ss = "Метеоритный дождь";
                        return ss;
                    case 13:
                        ss = "Ядро лавы";
                        return ss;
                   
                    default:
                        return ss;
                }
            }
            else if(Cls == 1)
            {
                //Воин
                switch (i)
                {
                    case 0:
                        ss = "Дикие знания";
                        return ss;
                    case 1:
                        ss = "Круговая атака";
                        return ss;
                    case 2:
                        ss = "Раскол";
                        return ss;
                    case 3:
                        ss = "Жажда крови";
                        return ss;
                    case 4:
                        ss = "Бросок топора";
                        return ss;
                    case 5:
                        ss = "Призыв бури";
                        return ss;
                    case 6:
                        ss = "Ярость берсерка";
                        return ss;
                    case 7:
                        ss = "Вестник смерти";
                        return ss;
                    case 8:
                        ss = "Поющие мечи";
                        return ss;
                    case 9:
                        ss = "";
                        return ss;
                    case 10:
                        ss = "";
                        return ss;
                    case 11:
                        ss = "";
                        return ss;
                    case 12:
                        ss = "";
                        return ss;
                    case 13:
                        ss = "";
                        return ss;
                    default:
                        return ss;
                }
            }
            else if(Cls == 2)
            {
                //Друид
                switch (i)
                {
                    case 0:
                        ss = "Камнепад";
                        return ss;
                    case 1:
                        ss = "Воздушный канал";
                        return ss;
                    case 2:
                        ss = "Запутывание";
                        return ss;
                    case 3:
                        ss = "Покой";
                        return ss;
                    case 4:
                        ss = "Лесной пожар";
                        return ss;
                    case 5:
                        ss = "Вызов молнии";
                        return ss;
                    case 6:
                        ss = "Испарение";
                        return ss;
                    case 7:
                        ss = "Стена шипов";
                        return ss;
                    case 8:
                        ss = "Сила земля";
                        return ss;
                    case 9:
                        ss = "Смерч";
                        return ss;
                    case 10:
                        ss = "Гроза";
                        return ss;
                    case 11:
                        ss = "Очищение";
                        return ss;
                    case 12:
                        ss = "Гнев природы";
                        return ss;
                    case 13:
                        ss = "";
                        return ss;
                    default:
                        return ss;
                }
            }
                // Рыцарь
            else if (Cls == 3)
            {
                switch (i)
                {
                    case 0:
                        ss = "Выпад";
                        return ss;
                    case 1:
                        ss = "Божественное право";
                        return ss;
                    case 2:
                        ss = "Вызов";
                        return ss;
                    case 3:
                        ss = "Оглушение";
                        return ss;
                    case 4:
                        ss = "Подавление";
                        return ss;
                    case 5:
                        ss = "Услуга";
                        return ss;
                    case 6:
                        ss = "Отвага";
                        return ss;
                    case 7:
                        ss = "Повышение";
                        return ss;
                    case 8:
                        ss = "Набег!";
                        return ss;
                    case 9:
                        ss = "Бессоница";
                        return ss;
                    case 10:
                        ss = "Сирианский меч";
                        return ss;
                    case 11:
                        ss = "Рыцарство";
                        return ss;
                    case 12:
                        ss = "Аура паладина";
                        return ss;
                    case 13:
                        ss = "Божья кара";
                        return ss;
                    default:
                        return ss;
                }
            }
            return ss;
        }

        private void ForwardHer_Click(object sender, RoutedEventArgs e)
        {
            FordBack++;
            if (FordBack > 3)
                FordBack = 0;
            
            NameHero.Content = NameHeroes(FordBack);
            Name.Content = NameHeroes(FordBack);

        }

        private void BackHer_Click(object sender, RoutedEventArgs e)
        {
            FordBack--;
            if (FordBack < 0)
                FordBack = 3;
            
            NameHero.Content = NameHeroes(FordBack);
            Name.Content = NameHeroes(FordBack);
        }

        private string NameHeroes(int FB)
        {
            switch(FB)
            {
                case 0:
                    HerIcon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Mag1.png", UriKind.Relative)));
                    Icon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Mag1.png", UriKind.Relative)));
                    ContH.Text = "Маги в Puzzle Quest бывают двух видов: быстрые и мертвые. Отсиживаться в обороне, накапливая магическую энергию, или играть «от черепов», нанося повреждения преимущественно сбором комбинации камней, этому персонажу сложно. Небольшое количество здоровья, невысокий навык боя — все это заставляет мага играть быстро, стараясь как можно раньше нанести противнику максимальный ущерб. \nМаг — единственный класс, который может легко и дешево развить все четыре магических навыка, что, во-первых, облегчает ему изучение и использование чужих заклинаний, а во-вторых — обеспечивает запас маны в начале боя.";
                    return "Маг";
                case 1:
                    HerIcon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/War1.png", UriKind.Relative)));
                    Icon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/War1.png", UriKind.Relative)));
                    ContH.Text = "Маги обрушивают на врагов огненные шары, друиды раз за разом перекраивают поле боя, рыцари пекутся о своем благородстве... Ха! Да что они понимают в хорошей драке? То ли дело, широко размахнувшись верным топором, разогнаться и броситься на противника с воинственным кличем! \nПримерно такое впечатление производит воин в Puzzle Quest при первом знакомстве. Безудержный рубака, в арсенале которого нет ни единого исцеляющего заклинания.";
                    return "Воин";
                case 2:
                    HerIcon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Druid1.png", UriKind.Relative)));
                    Icon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Druid1.png", UriKind.Relative)));
                    ContH.Text = "Победа в бою в Puzzle Quest часто достается не самому сильному и не самому умелому, а самому удачливому. В самом деле, если у вашего героя в начале битвы две сотни единиц здоровья и он с головы до ног одет в лучшие вещи, то очень обидно, когда противник одну за другой собирает комбинации из четырех-пяти камней, получает призовые ходы, а вы можете только сидеть и удрученно смотреть на монитор. Что делать? Раз за разом бросаться в бой, надеясь, что вот сейчас уже точно повезет? Можно и так. Но гораздо интереснее было бы сделать так, чтобы удача в бою уступила место четкому планированию и расчету. И лучше всего для этого подойдет герой-друид.";
                    return "Друид";
                case 3:
                    HerIcon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Knight1.png", UriKind.Relative)));
                    Icon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Knight1.png", UriKind.Relative)));
                    ContH.Text = "Рыцарь — персонаж прямолинейный и подходящий для тех, кто любит побеждать за счет сбора комбинаций черепов, или тех, кто считает магию слишком ненадежным оружием. Основные навыки «Битва» и «Боевой дух»? Вот и будем увеличивать их, не обращая внимания на остальные! В результате получается настоящий «танк», наносящий удвоенные, а то и утроенные повреждения. Большинство противников он может просто взять измором — они будут лишь удрученно наблюдать за своей тающей полоской здоровья.";
                    return "Рыцарь";
                default :
                    return null;
            }
        }

        private void ForwardEnemy_Click(object sender, RoutedEventArgs e)
        {
            FordBackE++;
            if (FordBackE > maxEnemy)
                FordBackE = 0;

            NameEnemy.Content = NameEnemys(FordBackE);
        }
         
        private void BackEnemy_Click(object sender, RoutedEventArgs e)
        {
            FordBackE--;
            if (FordBackE < 0)
                FordBackE = maxEnemy;

            NameEnemy.Content = NameEnemys(FordBackE);
        }

        private string NameEnemys(int FB)
        {
            switch (FB)
            {
                case 0:
                    EnemyIcon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Monst/Troll.png", UriKind.Relative)));
                    ContE.Text = "Тролли могут использовать свою Голубую Ману, чтобы исцеляться. Пока у него есть достаточно маны, тролль может регенерироваться за свой ход столько, сколько захочет.";
                    return "Тролль";
                case 1:
                    EnemyIcon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Monst/Skelet.png", UriKind.Relative)));
                    ContE.Text = "Скелеты могут не только высасывать вашу ману Хладной хваткой, но и Разбудить мертвых, превращая обычные черепа в мощные черепа.";
                    return "Скелет";

                case 2:
                    EnemyIcon.Background = new ImageBrush(new BitmapImage(new Uri("images/Icon/Monst/KingLich.png", UriKind.Relative)));
                    ContE.Text = "Верховные личи, как и личи, имеют способность и предметы для создания черепов. У них также высокая защита от огня и они могут использовать заклинание Обен Душами, чтобы обмениваться маной с противником. В дополнение ко всему, у Верзовных личей есть предметы, которые уменьшает ваши очки опыта.";
                    return "Король Лич";
                default :
                    return null;
            }
        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {     
            Close();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 14; i++)
            {
                Ch[i] = CreateCh(i);
                StackP.Children.Add(Ch[i]);
                Marg(Ch[i]);

                if (Ch[i].Content.Equals(""))
                    StackP.Children.Remove(Ch[i]);                
            }
            SetTool(FordBack);
            Ides_Name.Visibility = Visibility.Hidden;
            SpellsClass.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 14; i++ )
            {
                StackP.Children.Remove(Ch[i]);     
            }

            for (int i = 0; i < 5; i++ )
            {
                Spells[i] = -1;
            }
            ColvoSpell.Content = "0/5";
            NumberTS = 0;
            Ides_Name.Visibility = Visibility.Visible;
            SpellsClass.Visibility = Visibility.Hidden;
        }

        private void Fight_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();

            main.IdHero = FordBack;
            main.MonsterNum = FordBackE;
            main.HpValueEn = HpIndeff(FordBackE);
            
            for (int i = 0; i < 5; i++)
            {
                main.SpellСhoice[i] = Spells[i];
            }
            
            main.Show();
            setCreatingForm.Close();

            Close();
        }

        //Опеределяем кол-во здоровья врага;
        private int HpIndeff(int FordBack)
        {
            switch(FordBack)
            {
                case 0:         
                    return 110;
                case 1:
                    return 40;
                case 2:
                    return 1200;
                default :
                    return 0;
            }
        }

        public Window setCreatingForm { get; set;}

        //Если выбираем скилл;
        private void Ch_Checked(object sender, RoutedEventArgs e)
       {
           var ob = sender as CheckBox;
           Spells[NumberTS] = Int32.Parse("" + ob.Tag);
           NumberTS++;
           if(NumberTS >= 5)
           {
               EnNoCh(true);
           }

           ColvoSpell.Content = "" + NumberTS + "/5";
       }
        
        private void EnNoCh(Boolean tf)
       {
           if (tf)
           {
             for(int i = 0; i < 14; i++)
             {
                 if(Ch[i].IsChecked == false)
                 {
                     Ch[i].IsEnabled = false;
                 }
             }
           }
           else
           {
             for(int i = 0; i < 14; i++)
             {
                 Ch[i].IsEnabled = true;
             }
           }
       }

        
        //Если убираем скилл из списка;
        private void Ch_Unchecked(object sender, RoutedEventArgs e)
        {
            var ob = sender as CheckBox;
            if (NumberTS >=5)
            {
                EnNoCh(false);
            }
            NumberTS--;
            ColvoSpell.Content = "" + NumberTS + "/5";

            if (NumberTS > 0)
            {
                //Находим чек бокс который был Unchecked;
                for (int i = 0; i < 5; i++)
                {
                    if (Spells[i] == Int32.Parse("" + ob.Tag))
                    {
                        Spells[i] = -1;
                    }
                }

                //Смещаем скиллы;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (Spells[j] == -1)
                        {
                            int temp = Spells[j];
                            Spells[j] = Spells[j + 1];
                            Spells[j + 1] = temp;
                        }
                    }
                }
            }
            else
            {
                for(int i = 0; i < 5; i++)
                {
                    Spells[i] = -1;
                }
            }
        }

    }
}
