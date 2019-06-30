using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Puzzle_Quest__AnimeEdition_
{
    class SpellClass
    {

        public Random randomGenerator = new Random();

        /// <Спеллы> ///

        public int DamageSpell = 0; // DMG по герою;

        public Boolean TryDamage = true; // (Стена огня) 
        public Boolean[] BlindTF = new Boolean[2] { false, false }; //Проверка на ослепление;

        // << Ayra >> //
        // Маг //
        public int[] ShealdSpell = new int[2] { 0, 0 }; // Щит героя;
        public int[] Speshka = new int[2] { 0, 0 }; // Евент атака;
        public int[] HandPower = new int[2] { 0, 0 }; // Рука силы;

        public int blindness = 0; // Ослепление;
        public Boolean WallFire = false; //Стена огня;
        // /Маг //
        

        // Воин //
        public int SwordsTurn = 0;
        // /Воин //

        // Друид и Рыцарь (Оглушение (пропуск хода)) //  
        public int Entang = 0;
        // /Рыцарь//

        public Boolean WallSpike = false; //Стена шипов;
        // /Друид //
        

        // Рыцарь //
        public int FightPal = 0;
        public int Trade = 0; // Услуга;
        public int Insomnia = 0; //Бессоница;
        public int AyraPal = 0; // Аура паладина;
        // /Рыцарь //

        // <<\ Ayra />> //

        public Boolean CheckSquare = false; // Проверка квадрата;
        public Boolean CheckKrest = false; // Проверка креста;
        public Boolean CircularAttack = false; // Проверка на Круговую атаку;
        public Boolean ColummDestroid = false; // Проверка на колонку;
        public Boolean GrozaDestro = false; // Проверяем на грозу;
        public Boolean StoneDes = false; // Проверяем на камень;
        public Boolean UpPal = false; // Проверка на повышение;
        public Boolean RowDestroid = false; // Проверка на ряд;

        public Rectangle[] SquareKrest = new Rectangle[9]; // Запоминание области для ауе атаки;
        
        /// </Спеллы> ///

        public Button LockSpell { get; set; } // Блокирует способность на определенный срок;
        public int LockTime { get; set; } // Устанавливает длительность блокировки способности;
     

         public SpellClass(MainWindow main)
         {
            this.game = main;
         }

         MainWindow game;
         BrainBot botSpell;


        /// Устанавливаем подсказку для способностей;
         public static string JustFindFileTool(string NameS, int idH)
         {
             string textTool = "";
             string nameFile = "";

             if (idH == 0)      
                 nameFile = "Hz3Mag.txt";
             
             else if(idH == 1)
                 nameFile = "Hz3War.txt";

             else if(idH == 2)
                 nameFile = "Hz3Druid.txt";

             else if(idH == 3)
                 nameFile = "Hz3Knight.txt";

             

                string[] text = File.ReadAllLines(nameFile, Encoding.Default);
                int tempI = 0;
                int tempR = 0;

                   
                for (int i = 0; i < text.Length; i++)
                {
                     
                    if (text[i].Contains(NameS))
                    {
                        tempI = i;
                            
                        break;
                    }
                 }
                     
                 for (int i = tempI; i < text.Length; i++)
                 {

                    if (text[i].Contains("#"))
                        {
                            tempR = i;
                            break;
                        }
                 }

                 for (int i = tempI; i < tempR; i++ )
                 {
                    if(i == tempR - 1)
                        textTool += text[i];
                    else
                        textTool += text[i] + "\n";
                 }
                    return textTool;          
          }
         

         public void CheckValueManaZatrat(Label G, Label R, Label Y, Label B, int NumSpell, Button bt)
         {
             if (G.Foreground != Brushes.Red && R.Foreground != Brushes.Red &&
                Y.Foreground != Brushes.Red && B.Foreground != Brushes.Red)
             {
                 //Маг
                 if(game.IdHero == 0)
                 {
                     MinusValueManaMag(G, R, Y, B, NumSpell, bt);
                 }

                 //Воин
                 else if(game.IdHero == 1)
                 {
                     MinusValueManaWar(G, R, Y, B, NumSpell, bt);
                 }

                 //Друид
                 else if (game.IdHero == 2)
                 {
                     MinusValueManaDruid(G, R, Y, B, NumSpell, bt);
                 }

                 //Рыцарь
                 else if (game.IdHero == 3)
                 {
                     MinusValueManaKnight(G, R, Y, B, NumSpell, bt);
                 }

                 game.JustCheckMana();
             }
         }

         //Способности Рыцаря;
         private void MinusValueManaKnight(Label G, Label R, Label Y, Label B, int n, Button bt)
         {
             game.OpaF5();
             
             switch(n)
             {
                 //Выпад
                 case 0:
                     StoneDes = true;
                     break;

                 //Божественное право
                 case 1:
                    

                     game.yellowP.Value -= 6;
                     game.blueP.Value -= 6;

                     ValueL(true);

                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {                           
                             if (game.Rec[i, j].Tag.Equals(6))
                             {
                                 game.Brain(1, game.Rec[i, j], true);
                                 game.Rec[i, j].Tag = "0";
                                 game.Rec[i, j].Fill = null;
                                 
                             }
                         }
                     }

                     game.MoveBlockAuto();

                     game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     break;

                 //Вызов
                 case 2:
                     game.redP.Value -= 6;
                     game.yellowP.Value -= 6;

                     ValueL(true);

                     FightPal = 6;

                     Buffs(9);

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Оглушение
                 case 3:                   

                     LockSpell = bt;
                     LockTime = 3;

                     SreachNameBut();

                     Entang = 1;

                     int GzDmg = (int)game.redP.Value / 8;
                     game.Damage(5 + GzDmg, game.TurnHeroAndEnemy);


                     game.greenP.Value -= 6;
                     game.redP.Value -= 5;

                     ValueL(true);
                     break;

                 //Подавление 
                 case 4:
                     game.Damage((int)(game.greenP.Value - 1) / 2, true);
                     game.greenP.Value -= 7;
                     game.redP.Value -= 7;

                     ValueL(true);

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                     // Услуга
                 case 5:
                     Trade = (int)((game.yellowP.Value - 1) / 6) + 8;

                     game.yellowP.Value -= 6;
                     game.blueP.Value -= 8;

                     ValueL(true);
                     Buffs(10);

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Отвага
                 case 6:
                     if(game.blueP.Value - 1 < 10)
                         game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     game.redP.Value -= 2;
                     game.yellowP.Value -= 2;
                     game.blueP.Value -= 2;

                     ValueL(true);

                     closeAllBuffs();
                     break;

                 //Повышение
                 case 7:
                     UpPal = true;
                     LockSpell = bt;
                     LockTime = 3;
                     break;

                 //Набег!
                 case 8:
                     RowDestroid = true;
                     break;

                 //Бессоница
                 case 9:

                     Insomnia = (int)((game.blueP.Value - 1) / 5) + 8;
                     game.yellowP.Value -= 6;
                     game.blueP.Value -= 8;

                     ValueL(true);

                     Buffs(11);
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Сирианский меч
                 case 10:

                     game.Damage((int)game.yellowP.Value - 1 , true);

                     game.redP.Value -= 10;
                     game.greenP.Value -= 10;
                     game.blueP.Value -= 10;
                     game.yellowP.Value = 1;

                     ValueL(true);

                     LockSpell = bt;
                     LockTime = 2;
                     SreachNameBut();

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     
                     break;

                 //Рыцарство 
                 case 11:

                     game.yellowP.Value = game.yellowP.Maximum;
                     game.redP.Value = game.redP.Maximum;
                     game.greenP.Value = game.greenP.Maximum;
                     game.blueP.Value = game.blueP.Maximum;

                     game.yellowPE.Value = game.yellowPE.Maximum;
                     game.redPE.Value = game.redPE.Maximum;
                     game.greenPE.Value = game.greenPE.Maximum;
                     game.bluePE.Value = game.bluePE.Maximum;

                     ValueL(true);
                     ValueL(false);

                     closeAllBuffs();

                     LockSpell = bt;
                     LockTime = 3;
                     break;

                 //Аура паладина 
                 case 12:
                     AyraPal = (int)((game.yellowP.Value - 1) / 5) + 8;
                     Buffs(12);

                     game.yellowP.Value -= 12;
                     game.blueP.Value -= 9;
                     ValueL(true);

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Божья кара                
                 case 13:
                    CheckKrest = true;

                     LockSpell = bt;
                     LockTime = 3;

                     break;
             }
         }

         //Способности Друида;
         private void MinusValueManaDruid(Label G, Label R, Label Y, Label B, int n, Button bt)
         {
             game.OpaF5();

             switch(n)
             {
                     //Камнепад
                 case 0:
                     game.yellowP.Value -= 4;
                     int HpB = (int)(game.blueP.Value - 1) / 4;
                     game.blueP.Value -= 6;

                     game.heroP.Value += (HpB + 4);
                     game.heroL.Content = "" + game.heroP.Value;

                     ValueL(true);
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     break;

                 //Воздушный канал
                 case 1:
                     game.redP.Value -= 3;
                     game.greenP.Value -= 3;
                     game.blueP.Value -= 3;

                     game.yellowP.Value += 5;
                     ValueL(true);

                     game.JustCheckMana();
                     break;

                  //Запутывание
                 case 2:
                     int PlusTurnEng = (int)(game.greenP.Value - 1) / 20;
                     game.greenP.Value -= 12;
                     game.yellowP.Value -= 12;
                     ValueL(true);

                     Entang = (1 + PlusTurnEng);

                     BuffsEn(7);

                     LockSpell = bt;
                     LockTime = 4;
                     SreachNameBut();
                     break;

                 //Покой
                 case 3:

                     if(game.blueP.Value - 1 < 10)
                         game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     game.yellowP.Value -= 2;
                     game.blueP.Value -= 3;

                     closeAllBuffs();
                     ValueL(true);
                     break;

                 //Лесной пожар
                 case 4:
                     int PlusRedDmg =(int)(game.redP.Value - 1) / 4;
                     game.redP.Value -= 6;
                     game.yellowP.Value -= 8;
                     ValueL(true);
                     game.Damage(6 + PlusRedDmg, game.TurnHeroAndEnemy);

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Вызов молнии
                 case 5:
                     ColummDestroid = true;
                     break;

                 //Испарение 
                 case 6:

                     game.redP.Value -= 5;
                     game.blueP.Value -= 3;

                     ValueL(true);

                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если голубой блок
                             if ( game.Rec[i, j].Tag.Equals(4))
                             {
                                 game.Rec[i, j].Fill = game.Brushes_Rand(2, game.Rec[i, j]);
                                 
                             }
                         }
                     }

                     game.whileBlock();

                     //Lock
                     LockSpell = bt;
                     LockTime = 3;
                     SreachNameBut();

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                     //Стена шипов
                 case 7:

                     game.blueP.Value -= 9;
                     game.greenP.Value -= 6;
                     ValueL(true);
                     TryDamage = false;
                     Buffs(8);

                     WallSpike = true;
                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     break;

                 //Сила земля
                 case 8:
                     game.redP.Value -= 7;
                     game.blueP.Value -= 7;
                     game.yellowP.Value -= 7;

                     int grayGernia = 0;
                     ValueL(true);

                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если зеленный блок
                             if (game.Rec[i, j].Tag.Equals(3))
                             {
                                 game.Brain(1, game.Rec[i, j], true);
                                 game.Rec[i, j].Fill = null;
                                 grayGernia++;
                             }
                         }
                     }

                     game.heroP.Value += (int)grayGernia / 3;
                     game.heroL.Content = game.heroP.Value;
                     game.MoveBlockAuto();

                     game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     break;

                 //Смерч
                 case 9:
                     game.redP.Value -= 7;
                     game.blueP.Value -= 7;
                     game.greenP.Value -= 7;

                     grayGernia = 0;
                     ValueL(true);

                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если желтый блок
                             if (game.Rec[i, j].Tag.Equals(2))
                             {
                                 game.Brain(1, game.Rec[i, j], true);
                                 game.Rec[i, j].Fill = null;
                                 grayGernia++;
                             }
                         }
                     }

                     game.Damage((int)grayGernia / 3,true);
                     game.MoveBlockAuto();

                     game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     break;

                 //Гроза
                 case 10:
                     GrozaDestro = true;
                     break;

                 //Очищение
                 case 11:
                     game.yellowP.Value -= 9;
                     game.blueP.Value -= 9;
                     ValueL(true);

                     LockSpell = bt;
                     LockTime = 6;
                     SreachNameBut();

                     for(int i = 0; i < 8; i++)
                     {
                         for(int j = 0; j < 8; j++)
                         {
                             game.Rec[i, j].Fill = null;
                         }
                     }
                     MessageBox.Show("ХЫХ");
                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             game.Rec[i, j].Fill = game.Brushes_Rand(randomGenerator.Next(1,8),game.Rec[i,j]);
                         }
                     }
                     MessageBox.Show("ХЫХ2");
                     game.StartGame = false;
                     game.MoveBlockAuto();
                     game.whileBlock();
                     MessageBox.Show("ХЫХ3");
                     game.StartGame = true;

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                     //Гнев природы
                 case 12:
                     break;
             }
         }

         //Cпособности Воина;
         private void MinusValueManaWar(Label G, Label R, Label Y, Label B, int n, Button bt)
         {
             game.OpaF5();
             switch (n)
             {
                 //Дикие знания
                 case 0:
                     int PlusExp = 0;
                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если нашли желтый или голубой блок
                             if (game.Rec[i, j].Tag.Equals(2) || game.Rec[i,j].Tag.Equals(4))
                             {
                                 game.Rec[i, j].Tag = "0";
                                 game.Rec[i, j].Fill = null;
                                 PlusExp++;
                             }
                         }
                     }
                     if (PlusExp > 0)
                     {
                         game.yellowP.Value -= 8;
                         game.blueP.Value -= 8;
                         ValueL(true);
                         //Плюс к опыту

                         game.expL.Content = Int32.Parse("" + game.expL.Content) + PlusExp;
                         MessageBox.Show("ОПА Ф " + PlusExp);

                         game.MoveBlockAuto();

                         game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     }
                     break;


                 //Круговая атака
                 case 1:
                     CircularAttack = true;                  
                     break;

                 //Раскол
                 case 2:
                     int DmgEn = 0;

                     //Поиск нужных блоков;
                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если нашли желтый или голубой блок
                             if (game.Rec[i, j].Tag.Equals(2) )
                             {
                                 game.Rec[i, j].Tag = "0";
                                 game.Rec[i, j].Fill = null;
                                 DmgEn++;
                             }
                         }
                     }
                     if (DmgEn > 0)
                     {
                         game.yellowP.Value -= 4;
                         game.redP.Value -= 7;
                         ValueL(true);

                         game.Damage(DmgEn, game.TurnHeroAndEnemy);

                         MessageBox.Show("НЫЫЫЫЫЫЫЫЫЫЫЫЫЫЫЫЫАААААААААААААА  " + DmgEn);

                         game.MoveBlockAuto();

                         game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     }
                     break;

                     //Жажда крови
                 case 3:
                     game.greenP.Value -= 5;
                     game.yellowP.Value -= 3;
                     game.blueP.Value -= 5;

                     game.redP.Value += 12;

                     ValueL(true);

                     game.JustCheckMana();
                     break;

                 //Бросок топора

                 case 4:
                     int DmgEnm = 0;

                     //Поиск нужных блоков;
                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если нашли желтый или голубой блок
                             if (game.Rec[i, j].Tag.Equals(5))                                                          
                                 DmgEnm++;                           
                         }
                     }

                     game.Damage(4 + DmgEnm, game.TurnHeroAndEnemy);
                     MessageBox.Show("УУУУ СУКА, ИДИ СЮДА ПАДЛА " + DmgEnm);

                     game.greenP.Value -= 6;
                     game.redP.Value -= 8;
                     ValueL(true);

                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                     //Призыв бури
                 case 5:
                     game.redP.Value -= 8;
                     game.yellowP.Value -= 5;
                     game.blueP.Value -= 5;
                     ValueL(true);
                     int CheckTemp = -1;
                     for (int i = 0; i < 2; i++)
                     {
                     GZ:
                         int tempIs = randomGenerator.Next(0,8);
                         if(tempIs == CheckTemp)
                         {
                             goto GZ;
                         }
                         else 
                             CheckTemp = tempIs;
                         for(int j = 0; j < 8; j++)
                         {
                             game.Brain(1, game.Rec[j,tempIs], game.TurnHeroAndEnemy);
                             game.Rec[j, tempIs].Fill = null;
                             game.Rec[j, tempIs].Tag = "0";
                         }
                         MessageBox.Show("ОПА " + i);
                     }

                      game.MoveBlockAuto();

                      game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     break;

                 //Ярость берсерка
                 case 6:
                     //Поиск нужных блоков;
                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если нашли красный блок
                             if (game.Rec[i, j].Tag.Equals(1))
                             {
                                 game.Rec[i, j].Fill = game.Brushes_Rand(5, game.Rec[i, j]);                                
                             }

                         }
                     }

                     if(game.redP.Value - 1 >= 15)
                     {
                         game.whileBlock();
                     }
                     else
                     {
                         game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     }

                     game.greenP.Value -= 6;
                     game.redP.Value -= 10;

                     ValueL(true);

                     LockSpell = bt;
                     LockTime = 4;
                     SreachNameBut();
                     
                     break;

                 // Вестник смерти
                 case 7:
                     int Skulls = (int)(game.redP.Value - 1) / 3;
                     int MaxSku = 9;
                     for(int i = 0; i < Skulls; i++)
                     {
                     Gz:
                         int a = randomGenerator.Next(0, 8);
                         int b = randomGenerator.Next(0, 8);
                            if(game.Rec[a,b].Tag.Equals(5))
                            {
                                goto Gz;
                            }
                            else
                            {
                                game.Rec[a, b].Fill = game.Brushes_Rand(5, game.Rec[a, b]);
                            }

                            MessageBox.Show("ЧЕРЕП" + i);
                            if (MaxSku == 0)
                                break;
                            else
                                MaxSku--;
                            
                     }

                     MessageBox.Show("УСЁ");

                     game.greenP.Value -= 6;
                     game.redP.Value -= 15;
                     game.yellowP.Value -= 6;
                     game.blueP.Value -= 6;

                     ValueL(true);

                     LockSpell = bt;
                     LockTime = 4;
                     SreachNameBut();

                     game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     break;

                     //Поющие мечи
                 case 8:
                     game.redP.Value -= 8;
                     game.yellowP.Value -= 6;

                     ValueL(true);

                     int PlusSword = (int)(game.yellowP.Value - 1) / 2;

                     SwordsTurn = 5 + PlusSword;

                     Buffs(6);

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;
             }       
         }

         //Способности Мага;
         public void MinusValueManaMag(Label G, Label R, Label Y, Label B, int n, Button bt)
         {
             CheckSquare = false;

             game.OpaF5();

             switch (n)
             {
                 // огненная стрела
                 case 0:

                     int Tn = (int)game.redP.Value / 8;
                     game.redP.Value = game.redP.Value - 4;
                     game.yellowP.Value = game.yellowP.Value - 4;

                     game.redL.Content = "" + (game.redP.Value - 1);
                     game.yellowL.Content = "" + (game.yellowP.Value - 1);

                     game.Damage(4 + Tn, game.TurnHeroAndEnemy);

                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     break;

                 // щит
                 case 1:

                     int SpN = (int)(game.redP.Value - 1) / 3;

                     game.redP.Value = game.redP.Value - 5;
                     game.blueP.Value = game.blueP.Value - 6;

                     game.redL.Content = "" + (game.redP.Value - 1);
                     game.blueL.Content = "" + (game.blueP.Value - 1);

                     ShealdSpell[1] = 9 + SpN; // Кол-во ходов которое будет длиться Щит; 

                     Buffs(2);

                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     break;

                 // спешка
                 case 2:

                     int SpN2 = (int)(game.yellowP.Value - 1) / 5;

                     game.greenP.Value = game.greenP.Value - 2;
                     game.yellowP.Value = game.yellowP.Value - 6;

                     game.greenL.Content = "" + (game.greenP.Value - 1);
                     game.yellowL.Content = "" + (game.yellowP.Value - 1);

                     Speshka[1] = 9 + SpN2;

                     Buffs(1);

                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 // квадрат
                 case 3:

                     CheckSquare = true;
                     LockSpell = bt;
                     LockTime = 2;

                     break;

                 //Огненный канал
                 case 4:
                     game.greenP.Value -= 3;
                     game.yellowP.Value -= 3;
                     game.blueP.Value -= 3;
                     game.greenL.Content = game.greenP.Value - 1;
                     game.yellowL.Content = game.yellowP.Value - 1;
                     game.blueL.Content = game.blueP.Value - 1;

                     game.redL.Content = game.redP.Value += 5; ;
                     break;

                 //Рука силы;
                 case 5:
                     game.greenP.Value -= 5;
                     game.redP.Value -= 2;
                     game.yellowP.Value -= 7;

                     game.greenL.Content = game.greenP.Value - 1;
                     game.redL.Content = game.redP.Value - 1;
                     game.yellowL.Content = game.yellowP.Value - 1;

                     HandPower[1] = 6;
                     HandPower[0] += 2;

                     Buffs(3);

                     //Lock
                     LockSpell = bt;
                     LockTime = 2;
                     SreachNameBut();

                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Утечка тепла
                 case 6:
                     game.greenP.Value -= 3;
                     game.blueP.Value -= 3;
                     game.greenL.Content = game.greenP.Value - 1;
                     game.blueL.Content = game.blueP.Value - 1;
                     if ((int)(game.redPE.Value - 1) / 8 >= 1)
                     {
                         game.redPE.Value -= 8;
                         game.redLE.Content = game.redPE.Value - 1;
                         game.redL.Content = game.redP.Value += 8;
                     }
                     else
                     {
                         game.redP.Value += game.redPE.Value - 1;
                         game.redL.Content = game.redP.Value - 1;
                         game.redPE.Value = 1;
                         game.redLE.Content = game.redPE.Value - 1;
                     }

                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);

                     break;

                 //Стена огня
                 case 7:

                     game.redP.Value -= 6;
                     game.greenP.Value -= 9;
                     ValueL(true);
                     TryDamage = false;
                     Buffs(5);

                     WallFire = true;
                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Сжигание маны;
                 case 8:

                     if (game.greenPE.Value - 5 <= 0)
                         game.greenPE.Value = 1;
                     else
                         game.greenPE.Value -= 5;

                     if (game.redPE.Value - 5 <= 0)
                         game.redPE.Value = 1;
                     else
                         game.redPE.Value -= 5;

                     if (game.yellowPE.Value - 5 <= 0)
                         game.yellowPE.Value = 1;
                     else
                         game.yellowPE.Value -= 5;

                     if (game.bluePE.Value - 5 <= 0)
                         game.bluePE.Value = 1;
                     else
                         game.bluePE.Value -= 5;

                     ValueL(false);

                     game.greenP.Value -= 3;
                     game.greenL.Content = game.greenP.Value - 1;
                     game.redP.Value -= 4;
                     game.redL.Content = game.redP.Value - 1;

                     game.JustCheckManaBot();

                     if (game.redP.Value - 4 < 1)
                         game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Свет
                 case 9:

                     int PlusTurnV = (int)game.yellowP.Value / 8;
                     game.yellowP.Value -= 10;
                     game.redP.Value -= 6;

                     ValueL(true);

                     blindness = 1 + PlusTurnV;
                     BuffsEn(4);
                     BlindTF[0] = true;
                     //Lock
                     LockTime = 3;
                     LockSpell = bt;
                     SreachNameBut();

                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;
                 //Горящие черепа
                 case 10:

                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если нашли зеленный блок
                             if (game.Rec[i, j].Tag.Equals(3))
                             {
                                 game.Rec[i, j].Fill = game.Brushes_Rand(5, game.Rec[i, j]);
                             }
                             //Если нашли синий блок
                             else if (game.Rec[i, j].Tag.Equals(4))
                             {
                                 game.Rec[i, j].Fill = game.Brushes_Rand(1, game.Rec[i, j]);
                             }
                         }
                     }
                     game.greenP.Value -= 4;
                     game.redP.Value -= 9;
                     game.yellowP.Value -= 4;
                     game.blueP.Value -= 3;
                     ValueL(true);

                     MessageBox.Show("Чистим чистим");
                     game.MetodCheckBlock();

                     LockSpell = bt;
                     LockTime = 3;
                     SreachNameBut();

                     break;

                 //Прижигание
                 case 11:
                     int PlusHp = 0;
                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             //Если нашли красный блок
                             if (game.Rec[i, j].Tag.Equals(1))
                             {
                                 PlusHp++;
                             }
                         }
                     }
                     game.redP.Value -= 6;
                     game.blueP.Value -= 8;
                     ValueL(true);
                     game.heroP.Value += PlusHp;
                     game.heroL.Content = game.heroP.Value;
                     MessageBox.Show("Исцеление на " + PlusHp);

                     //Смена хода
                     game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                     break;

                 //Метеоритный дождь
                 case 12:

                     game.greenP.Value -= 10;
                     game.redP.Value -= 14;
                     game.yellowP.Value -= 6;
                     ValueL(true);

                     for (int i = 0; i < 8; i++)
                     {
                     GoNext:

                         int x = randomGenerator.Next(0, 8);
                         int y = randomGenerator.Next(0, 8);
                        if (game.Rec[x, y].Fill != null)
                         {
                             game.Brain(1, game.Rec[x, y], true);
                             game.Rec[x, y].Fill = null;
                         }
                         else
                             goto GoNext;
                     }
                     MessageBox.Show("It`s Working");
                     for (int i = 0; i < 8; i++)
                     {
                         for (int j = 0; j < 8; j++)
                         {
                             if (game.Rec[i, j].Fill == null)
                             {
                                 game.Rec[i, j].Fill = game.Brushes_Rand(1, game.Rec[i, j]);
                             }
                         }
                     }
                     game.CheckToDestroidBlock(game.TurnHeroAndEnemy);
                     break;

                 //Ядро лавы
                 case 13:

                     game.redP.Maximum += 12;
                     game.redP.Value -= 24;
                     game.greenP.Value -= 12;
                     ValueL(true);


                     break;
                 default:
                     MessageBox.Show("Скоро");
                     break;
             }
         }

         //Отображение маны (Label) ;
         public void ValueL(Boolean bel)
         {
             if (bel)
             {
                 game.redL.Content = game.redP.Value - 1;
                 game.greenL.Content = game.greenP.Value - 1;
                 game.yellowL.Content = game.yellowP.Value - 1;
                 game.blueL.Content = game.blueP.Value - 1;
             }
             else
             {
                 game.redLE.Content = game.redPE.Value - 1;
                 game.greenLE.Content = game.greenPE.Value - 1;
                 game.yellowLE.Content = game.yellowPE.Value - 1;
                 game.blueLE.Content = game.bluePE.Value - 1;
             }
         }

        //Убираем все иконки баффов;
        private void closeAllBuffs()
         {
            for(int i = 0; i < 7; i++)
            {
                game.BFHer[i].Content = "";
                game.BFHer[i].Tag = "0";
                game.BFHer[i].Visibility = Visibility.Hidden;

                game.BFEnemy[i].Content = "";
                game.BFEnemy[i].Tag = "0";
                game.BFEnemy[i].Visibility = Visibility.Hidden;
            }
            // Воин //
            SwordsTurn = 0;

            // Друид //
            Entang = 0;
            
            //Рыцарь //
            FightPal = 0;
            Trade = 0;
            Insomnia = 0;
            // Маг // 
            //Ослепление
            blindness = 0; 

            ShealdSpell = new int[2] { 0, 0 }; // Щит героя;
            Speshka = new int[2] { 0, 0 }; // Евент атака;
            HandPower = new int[2] { 0, 0 }; // Рука силы;
            WallSpike = false;
            WallFire = false;

            TryDamage = true;
         }

         // Уменьшение длительности действия баффов врага;
         public void BuffsMunusTimeEnemy()
         {
             for (int i = 6; i >= 0; i--)
             {
                 if (game.BFEnemy[i].Visibility != Visibility.Hidden)
                 {
                     
                         if (Int32.Parse("" + game.BFEnemy[i].Content) - 1 == 0)
                         {
                             if (game.BFEnemy[i].Tag.Equals(7))
                                 game.BFEnemy[i].Foreground = Brushes.White;
                             game.BFEnemy[i].Tag = "0";
                             game.BFEnemy[i].Visibility = Visibility.Hidden;
                             MoveBuffsEn();
                         }
                         else
                         {
                             game.BFEnemy[i].Content = Int32.Parse("" + game.BFEnemy[i].Content) - 1;
                             game.BFEnemy[i].ToolTip = StringName(Int32.Parse("" + game.BFEnemy[i].Tag)) + "\n Длительность : " + game.BFEnemy[i].Content;
                         }
                 }
                 else
                     break;
             }
         }

         // Уменьшение длительности действия баффов героя;
         public void BuffsMinusTime()
         {
             Boolean tr = false;
             for (int i = 0; i < 7; i++)
             {
                 if (game.BFHer[i].Visibility != Visibility.Hidden)
                 {
                     if (!game.BFHer[i].Tag.Equals(5) && !game.BFHer[i].Tag.Equals(8))
                     {
                         if (Int32.Parse("" + game.BFHer[i].Content) - 1 == 0)
                         {
                             game.BFHer[i].Tag = "0";
                             
                             tr = true;
                         }
                         else
                         {
                             game.BFHer[i].Content = Int32.Parse("" + game.BFHer[i].Content) - 1;
                             game.BFHer[i].ToolTip = StringName(Int32.Parse("" + game.BFHer[i].Tag)) + "\n Длительность : " + game.BFHer[i].Content;
                         }
                     }
                         //Если Нашли стены (Баффы)
                     else
                     {
                         if(game.BFHer[i].Tag.Equals(5))
                         {
                             tr = WallMinus(tr, i, game.redP, game.redL);
                         }
                         else if(game.BFHer[i].Tag.Equals(8))
                         {
                             tr = WallMinus(tr, i, game.greenP, game.greenL);
                         }
                     }
                 }
                 else
                     break;
             }

             if (tr) { MoveBuffs(); }
         }

         private bool WallMinus(Boolean tr, int i, ProgressBar rg, Label rlgl)
         {
             if (rg.Value - 2 > 1)
             {
                 rg.Value -= 2;
                 rlgl.Content = rg.Value - 1;
                 game.JustCheckMana();
             }
             else
             {
                 TryDamage = true;
                 game.BFHer[i].Tag = "0";
                 tr = true;
             }
             return tr;
         }

         // Двигаем баффы врага;
         public void MoveBuffsEn()
         {
             for (int i = 6; i > 0; i--)
             {
                 if (game.BFEnemy[i].Tag.Equals("0"))
                 {
                     Label lb = new Label();

                     lb.Background = game.BFEnemy[i - 1].Background;
                     lb.Content = game.BFEnemy[i - 1].Content;
                     lb.Tag = game.BFEnemy[i - 1].Tag;

                     game.BFEnemy[i - 1].Background = game.BFEnemy[i].Background;
                     game.BFEnemy[i - 1].Tag = game.BFEnemy[i].Tag;

                     game.BFEnemy[i].Background = lb.Background;
                     game.BFEnemy[i].Content = lb.Content;
                     game.BFEnemy[i].Tag = lb.Tag;
                 }
             }
         }

         // Двигаем баффы героя;
         public void MoveBuffs()
         {
             for (int j = 0; j < 7; j++)
             {
                 for (int i = 0; i < 6; i++)
                 {
                     if (game.BFHer[i].Tag.Equals("0"))
                     {
                         Label lb = new Label();

                         lb.Content = game.BFHer[i + 1].Content;
                         lb.Tag = game.BFHer[i + 1].Tag;
                         lb.Background = game.BFHer[i + 1].Background;
                         lb.ToolTip = game.BFHer[i + 1].ToolTip;

                         game.BFHer[i + 1].Tag = game.BFHer[i].Tag;

                         game.BFHer[i].Content = lb.Content;
                         game.BFHer[i].Tag = lb.Tag;
                         game.BFHer[i].Background = lb.Background;
                         game.BFHer[i].ToolTip = lb.ToolTip;
                     }
                 }
             }

             for (int i = 0; i < 7; i++)
             {
                 if (game.BFHer[i].Tag.Equals("0"))
                     game.BFHer[i].Visibility = Visibility.Hidden;
             }
         }

         private string StringName(int p)
         {
             String s;
             switch (p)
             {
                 case 1:
                     s = "Спешка.";
                     return s;
                 case 2:
                     s = "Магический Щит.";
                     return s;
                 case 3:
                     s = "Рука Силы. + " + HandPower[0] + " К ущербу";
                     return s;
                 case 4:
                     s = "Ослепление.";
                     return s;

                     //War
                 case 6:
                     s = "Поющие мечи.";
                     return s;

                     //Druid
                 case 7:
                     s = "Запутывание.";
                     return s;

                     //Knight
                 case 9:
                     s = "Вызов.";
                     return s;
                     
                 case 10:
                     s = "Услуга.";
                     return s;

                 case 11:
                     s = "Бессоница.";
                         return s;

                 case 12:
                         s = "Аура паладина.";
                         return s;
                 default:
                     return null;
             }
         }

         public void OffLock(Button Clk, Label Locks)
         {
             try
             {
                 if (!Clk.IsEnabled)
                 {
                     if (Int32.Parse("" + Locks.Tag) - 1 > 0)
                     {
                         Locks.Tag = Int32.Parse("" + Locks.Tag) - 1;
                         Locks.ToolTip = "Заблокировано еще " + Locks.Tag + " ходов";
                     }
                     else
                     {
                         Locks.Tag = 0;
                         Locks.Visibility = Visibility.Hidden;
                         Clk.IsEnabled = true;
                         Clk.Background = Brushes.Transparent;
                         Clk.Opacity = 1;


                     }
                 }
             }
             catch (FormatException) { }
         }

  

         // Устанавливаем иконки баффов и их длительность(для героя);
         public void Buffs(int n)
         {
             for (int i = 0; i < 7; i++)
             {
                 if (Int32.Parse("" + game.BFHer[i].Tag) == 0 || Int32.Parse("" + game.BFHer[i].Tag) == n)
                 {
                     game.BFHer[i].Background = SetIconBuff(n, game.BFHer[i]);
                     break;
                 }
             }

         }

         //// Устанавливаем иконки баффов и их длительность(для врага);
         public void BuffsEn(int n)
         {
             for (int i = 6; i >= 0; i--)
             {
                 if (Int32.Parse("" + game.BFEnemy[i].Tag) == 0 || Int32.Parse("" + game.BFEnemy[i].Tag) == n)
                 {
                     game.BFEnemy[i].Background = SetIconBuff(n, game.BFEnemy[i]);
                     break;
                 }
             }

         }

         //Иконки баффов;
         public ImageBrush SetIconBuff(int Num, Label bf)
         {
             ImageBrush brush = new ImageBrush();
             bf.Tag = Num;
             bf.Visibility = Visibility.Visible;
             switch (Num)
             {
                 // Спешка 1;
                 case 1:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/Sprint.png", UriKind.Relative));
                     bf.Content = "" + Speshka[1];
                     bf.ToolTip = "Спешка.\n Длительность : " + bf.Content;
                     return brush;
                 // Маг щит 2;
                 case 2:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/MagShield.png", UriKind.Relative));
                     bf.Content = "" + ShealdSpell[1];
                     bf.ToolTip = "Магический Щит.\n Длительность : " + bf.Content;
                     return brush;
                 // Рука силы 3;
                 case 3:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/Hand.jpg", UriKind.Relative));
                     bf.Content = "" + HandPower[1];
                     bf.ToolTip = "Рука силы. +" + HandPower[0] + " К ущербу\n Длительность : " + bf.Content;
                     return brush;
                 //Ослепление 4;
                 case 4:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/Oslep.jpg", UriKind.Relative));
                     bf.Content = "" + blindness;
                     bf.ToolTip = "Ослепление. \n Длительность : " + bf.Content;
                     return brush;
                 //Стена огня 5;
                 case 5:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/WallFire.jpg", UriKind.Relative));
                     bf.ToolTip = "Стена Огня.";
                     return brush;
                 //Поющие мечи 6 (WAR);
                 case 6:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/Singingswords.jpg", UriKind.Relative));
                     bf.Content = "" + SwordsTurn;
                     bf.ToolTip = "Поющие мечи.\n Длительность : " + bf.Content;
                     return brush;
                 //Запутывание 7 (Druid)
                 case 7:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/Entanglement.jpg", UriKind.Relative));
                     bf.Content = "" + Entang;
                     bf.Foreground = Brushes.Transparent;
                     bf.ToolTip = "Запутывание.\n Длительность : " + bf.Content;
                     return brush;
                 //Стена шипов 8 (Druid)
                 case 8:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/spikewall.png", UriKind.Relative));
                     bf.ToolTip = "Стена шипов.";
                     return brush;
                     //Вызов 9(Knight)
                 case 9:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/Duel.jpg", UriKind.Relative));
                     bf.Content = "" + FightPal;
                     bf.ToolTip = "Вызов.\n Длительность : " + bf.Content; 
                     return brush;
                 //Услуга 10(Knight)
                 case 10:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/Trade.png", UriKind.Relative));
                     bf.Content = "" + Trade;
                     bf.ToolTip = "Услуга.\n Длительность : " + bf.Content; 
                     return brush;
                 //Бессоница 11(Knight)
                 case 11:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/inst.jpg", UriKind.Relative));
                     bf.Content = "" + Insomnia;
                     bf.ToolTip = "Бессоница.\n Длительность : " + bf.Content; 
                     return brush;
                 //Аура паладина 12(Knight)
                 case 12:
                     brush.ImageSource = new BitmapImage(new Uri("images/Icon_Buffs/AyraPal.jpg", UriKind.Relative));
                     bf.Content = "" + AyraPal;
                     bf.ToolTip = "Аура паладина.\n Длительность : " + bf.Content;
                     return brush;
                 default:
                     return null;
             }
         }

        //Услуга (Рыцарь);
         internal int TradePal()
         {
             return randomGenerator.Next(0, 2);
         }

         /// /Спеллы ///

         // LOCK //

         public void SreachNameBut()
         {
             if (LockSpell.Name.Equals(game.ClkSpell1.Name))
             {
                 game.Lock.Tag = LockTime;
                 game.Lock.Visibility = Visibility.Visible;
                 game.Lock.ToolTip = "Заблокировано еще " + game.Lock.Tag + " ходов";
             }
             else if (LockSpell.Name.Equals(game.ClkSpell2.Name))
             {
                 game.Lock1.Tag = LockTime;
                 game.Lock1.Visibility = Visibility.Visible;
                 game.Lock1.ToolTip = "Заблокировано еще " + game.Lock1.Tag + " ходов";
             }
             else if (LockSpell.Name.Equals(game.ClkSpell3.Name))
             {
                 game.Lock2.Tag = LockTime;
                 game.Lock2.Visibility = Visibility.Visible;
                 game.Lock2.ToolTip = "Заблокировано еще " + game.Lock2.Tag + " ходов";
             }
             else if (LockSpell.Name.Equals(game.ClkSpell4.Name))
             {
                 game.Lock3.Tag = LockTime;
                 game.Lock3.Visibility = Visibility.Visible;
                 game.Lock3.ToolTip = "Заблокировано еще " + game.Lock3.Tag + " ходов";
             }
             else if (LockSpell.Name.Equals(game.ClkSpell5.Name))
             {
                 game.Lock4.Tag = LockTime;
                 game.Lock4.Visibility = Visibility.Visible;
                 game.Lock4.ToolTip = "Заблокировано еще " + game.Lock4.Tag + " ходов";
             }

             LockSpell.IsEnabled = false;
             LockSpell.Background = Brushes.Black;
             LockSpell.Opacity = 0.2;
         }

        // /LOCK //


         //// Aye Attack ////

         // Запоминаем выбранную область (квадрат);
         public void Squares(Rectangle rec)
         {
             //  Минус мана //
             game.redP.Value = game.redP.Value - 9;
             game.yellowP.Value = game.yellowP.Value - 6;

             game.redL.Content = "" + (game.redP.Value - 1);
             game.yellowL.Content = "" + (game.yellowP.Value - 1);
             // Минус мана //

             // Lock;
             SreachNameBut();

             game.Damage(8, true);
             for (int i = 0; i <= 8; i++)
                 SquareKrest[i] = null;

             try { SquareKrest[0] = game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec) - 1]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[1] = game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[2] = game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec) + 1]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[3] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) - 1]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[4] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[5] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 1]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[6] = game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec) - 1]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[7] = game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[8] = game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec) + 1]; }
             catch (IndexOutOfRangeException)
             { }

             SquareOrKrestNull();
             ClearSquare(rec);
         }

         // Чистим область (Квадрат);
         public void ClearSquare(Rectangle rec)
         {

             game.OpaF5();

             try { game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec) - 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec) + 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) - 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec) - 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec) + 1].Fill = null; }
             catch (IndexOutOfRangeException) { }

             game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec)].Fill = null;

             game.MoveBlockAuto();

             game.MetodCheckBlock();


         }

        //Крест и квадрат
         public void SquareOrKrestNull()
         {
             for (int i = 0; i <= 8; i++)
             {
                 game.Brain(1, SquareKrest[i], true);
             }
         }

        //Крест
         public void Krest(Rectangle rec)
         {
             //  Минус мана //
             game.greenP.Value = game.greenP.Value - 5;
             game.redP.Value = game.redP.Value - 5;
             game.yellowP.Value = game.yellowP.Value - 3;
             game.blueP.Value = game.blueP.Value - 3;

             ValueL(true);

             // Минус мана //

             // Lock;
             SreachNameBut();

             game.Damage(8, true);
             for (int i = 0; i <= 8; i++)
                 SquareKrest[i] = null;

             try { SquareKrest[0] = game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[1] = game.Rec[Grid.GetRow(rec) - 2, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[2] = game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[3] = game.Rec[Grid.GetRow(rec) - 2, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[4] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[5] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 1]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[6] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 2]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[7] = game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[8] = game.Rec[Grid.GetRow(rec) + 2, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }

             SquareOrKrestNull();
             ClearKrest(rec);
         }

         public void ClearKrest(Rectangle rec)
         {


             game.OpaF5();

             try { game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) - 2, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) - 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) - 2].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) + 2, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 2].Fill = null; }
             catch (IndexOutOfRangeException) { }

             game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec)].Fill = null;
             MessageBox.Show("REC");
             game.MoveBlockAuto();

             game.MetodCheckBlock();
         }


        // Круговая атака
         internal void Circular(Rectangle rec)
         {
             game.redP.Value -= 6 ;
             game.greenP.Value -= 4 ;
             ValueL(true);

             game.Damage(3, true);
             for (int i = 0; i <= 8; i++)
                 SquareKrest[i] = null;

             try { SquareKrest[0] = game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[1] = game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[2] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 1]; }
             catch (IndexOutOfRangeException)
             { }
             try { SquareKrest[3] = game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) - 1]; }
             catch (IndexOutOfRangeException)
             { }

             CircularValue();

             ClearCircular(rec);
         }

         internal void ClearCircular(Rectangle rec)
         {
             game.OpaF5();

             try { game.Rec[Grid.GetRow(rec) - 1, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec) + 1, Grid.GetColumn(rec)].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) + 1].Fill = null; }
             catch (IndexOutOfRangeException) { }
             try { game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec) - 1].Fill = null; }
             catch (IndexOutOfRangeException) { }

             game.MoveBlockAuto();

             game.MetodCheckBlock();
         }

         private void CircularValue()
         {
            for(int i = 0; i < 4; i++)
            {
                game.Brain(1, SquareKrest[i], true);
            }
         }


         // Столбец
         internal void ColummsAndValue(Rectangle rec)
         {
             game.OpaF5();

             game.Damage(6, true);

             game.greenP.Value -= 6;
             game.yellowP.Value -= 8;
             game.blueP.Value -= 6;

             ValueL(true);

             for(int i = 0; i < 8; i++)
             {
                 game.Brain(1, game.Rec[i,Grid.GetColumn(rec)], game.TurnHeroAndEnemy);
                 game.Rec[i, Grid.GetColumn(rec)].Fill = null;
             }

             game.MoveBlockAuto();

             game.MetodCheckBlock();
         }



         //Гроза (друид)
         internal void GrozaDestroid(Rectangle rec)
         {
             game.OpaF5();

             game.Damage(16, true);

             game.greenP.Value -= 12;
             game.yellowP.Value -= 16;
             game.blueP.Value -= 12;

             ValueL(true);

             try
             {
                 for (int j = 0; j < 3; j++)
                 {
                     for (int i = 0; i < 8; i++)
                     {
                         SquareKrest[i] = game.Rec[i, (Grid.GetColumn(rec) + 1) - j ];
                         game.Brain(1, SquareKrest[i], game.TurnHeroAndEnemy);
                     }

                     for (int i = 0; i < 8; i++)
                     {
                         game.Rec[i, (Grid.GetColumn(rec) + 1) - j].Fill = null;
                     }
                     MessageBox.Show("G " + j);
                 }
             }
             catch (NullReferenceException) { }

             game.MoveBlockAuto();

             game.MetodCheckBlock();
         }


         //Stone (рыцарь)
         internal void StoneDestroid(Rectangle rec)
         {
             game.redP.Value -= 4;
             game.blueP.Value -= 2;

             ValueL(true);

             game.OpaF5();

             game.Brain(1, game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec)],true);
             game.Rec[Grid.GetRow(rec), Grid.GetColumn(rec)].Fill = null;

             game.MoveBlockAuto();

             game.MetodCheckBlock();
         }
         
        
         //Повышение (рыцарь)     
         internal void UpKnight(int Tags)
         {
             game.OpaF5();

             game.blueP.Value -= 8;
             game.yellowP.Value -= 8;
             game.redP.Value -= 8;
             game.greenP.Value -= 8;
             ValueL(true);

             for (int i = 0; i < 8; i++ )
             {
                 for(int j = 0; j < 8; j++)
                 {
                     if(game.Rec[i,j].Tag.Equals(Tags))
                     {
                         game.Rec[i, j].Fill = game.Brushes_Rand(6, game.Rec[i, j]);
                     }
                 }
             }

                 SreachNameBut();
             
                game.MetodCheckBlock();
         }

        
         // Ряд
         internal void RowDestr(Rectangle rec)
         {
             game.OpaF5();

             game.Damage(6, true);

             game.greenP.Value -= 6;
             game.yellowP.Value -= 6;
             game.blueP.Value -= 6;
             game.redP.Value -= 6;

             ValueL(true);

             for (int i = 0; i < 8; i++)
             {
                 game.Brain(1, game.Rec[Grid.GetRow(rec), i], game.TurnHeroAndEnemy);
                 game.Rec[Grid.GetRow(rec), i].Fill = null;
             }

             game.MoveBlockAuto();
             game.MetodCheckBlock();
         }

         /// /Aye Attack ///


         public void HidSpell(int inis)
         {
             if (inis == 0)
             {
                 game.ClkSpell1.Visibility = Visibility.Hidden;
                 game.NameSpell1.Visibility = Visibility.Hidden;
                 game.Spell1Grid.Visibility = Visibility.Hidden;
             }
             else if (inis == 1)
             {
                 game.ClkSpell2.Visibility = Visibility.Hidden;
                 game.NameSpell2.Visibility = Visibility.Hidden;
                 game.Spell2Grid.Visibility = Visibility.Hidden;
             }
             else if (inis == 2)
             {
                 game.ClkSpell3.Visibility = Visibility.Hidden;
                 game.NameSpell3.Visibility = Visibility.Hidden;
                 game.Spell3Grid.Visibility = Visibility.Hidden;
             }
             else if (inis == 3)
             {
                 game.ClkSpell4.Visibility = Visibility.Hidden;
                 game.NameSpell4.Visibility = Visibility.Hidden;
                 game.Spell4Grid.Visibility = Visibility.Hidden;
             }
             else if (inis == 4)
             {
                 game.ClkSpell5.Visibility = Visibility.Hidden;
                 game.NameSpell5.Visibility = Visibility.Hidden;
                 game.Spell5Grid.Visibility = Visibility.Hidden;
             }
         }

         public void IdSpellHer(int idH)
         {

             for (int i = 0; i < 5; i++)
             {
                 if (game.SpellСhoice[i] == -1)
                 {
                     HidSpell(i);
                 }
             }


             SkillsPanel(game.SpellСhoice[0], idH, game.NameSpell1, game.GreenManaLab, game.RedManaLab, game.YellowManaLab, game.BlueManaLab,
                     game.GreenManaText, game.RedManaText, game.YellowManaText, game.BlueManaText);

             SkillsPanel(game.SpellСhoice[1], idH, game.NameSpell2, game.GreenManaLab1, game.RedManaLab1, game.YellowManaLab1, game.BlueManaLab1,
                         game.GreenManaText1, game.RedManaText1, game.YellowManaText1, game.BlueManaText1);

             SkillsPanel(game.SpellСhoice[2], idH, game.NameSpell3, game.GreenManaLab2, game.RedManaLab2, game.YellowManaLab2, game.BlueManaLab2,
                         game.GreenManaText2, game.RedManaText2, game.YellowManaText2, game.BlueManaText2);

             SkillsPanel(game.SpellСhoice[3], idH, game.NameSpell4, game.GreenManaLab3, game.RedManaLab3, game.YellowManaLab3, game.BlueManaLab3,
                         game.GreenManaText3, game.RedManaText3, game.YellowManaText3, game.BlueManaText3);

             SkillsPanel(game.SpellСhoice[4], idH, game.NameSpell5, game.GreenManaLab4, game.RedManaLab4, game.YellowManaLab4, game.BlueManaLab4,
                         game.GreenManaText4, game.RedManaText4, game.YellowManaText4, game.BlueManaText4);

         }


         //Skills Brain Mag;
         public void SkillsPanel(int n, int id, Label Name, Label GL, Label RL, Label YL, Label BL,
             Label GT, Label RT, Label YT, Label BT)
         {
             //Маг
             if (id == 0)
             {
                 switch (n)
                 {
                     case 0:
                         game.ScaleManaTop(0, 4, 4, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 1:
                         game.ScaleManaTop(0, 5, 0, 6, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 2:
                         game.ScaleManaTop(2, 0, 6, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 3:
                         game.ScaleManaTop(0, 9, 6, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 4:
                         game.ScaleManaTop(3, 0, 3, 3, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 5:
                         game.ScaleManaTop(5, 2, 7, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 6:
                         game.ScaleManaTop(3, 0, 0, 3, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 7:
                         game.ScaleManaTop(9, 6, 0, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 8:
                         game.ScaleManaTop(3, 4, 0, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 9:
                         game.ScaleManaTop(0, 6, 10, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 10:
                         game.ScaleManaTop(4, 9, 4, 3, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 11:
                         game.ScaleManaTop(0, 6, 0, 8, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 12:
                         game.ScaleManaTop(10, 14, 6, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 13:
                         game.ScaleManaTop(12, 24, 0, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;
                 }
             }
             // Вар
             else if (id == 1)
             {
                 switch (n)
                 {
                     case 0:
                         game.ScaleManaTop(0, 0, 8, 8, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 1:
                         game.ScaleManaTop(4, 6, 0, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 2:
                         game.ScaleManaTop(0, 7, 4, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 3:
                         game.ScaleManaTop(5, 0, 3, 5, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 4:
                         game.ScaleManaTop(6, 8, 0, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 5:
                         game.ScaleManaTop(0, 8, 5, 5, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 6:
                         game.ScaleManaTop(6, 10, 0, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 7:
                         game.ScaleManaTop(6, 15, 6, 6, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 8:
                         game.ScaleManaTop(0, 8, 6, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;
                 }
             }
             //Друид
             else if (id == 2)
             {
                 switch (n)
                 {
                     case 0:
                         game.ScaleManaTop(0, 0, 4, 6, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 1:
                         game.ScaleManaTop(3, 3, 0, 3, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 2:
                         game.ScaleManaTop(12, 0, 12, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 3:
                         game.ScaleManaTop(0, 0, 2, 3, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 4:
                         game.ScaleManaTop(0, 6, 8, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 5:
                         game.ScaleManaTop(6, 0, 8, 6 , GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 6:
                         game.ScaleManaTop(0, 5, 0, 3, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 7:
                         game.ScaleManaTop(6, 0, 0, 9, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;


                     case 8:
                         game.ScaleManaTop(0, 7, 7, 7, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 9:
                         game.ScaleManaTop(7, 7, 0, 7, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 10:
                         game.ScaleManaTop(12, 0, 16, 12, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 11:
                         game.ScaleManaTop(0, 0, 9, 9 , GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 12:
                         game.ScaleManaTop(25, 0, 0, 10, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;
                 }
             }
                 //Рыцарь
             else if (id == 3)
             {
                 switch (n)
                 {
                     case 0:
                         game.ScaleManaTop(0, 4, 0, 2, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 1:
                         game.ScaleManaTop(0, 0, 6, 6, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 2:
                         game.ScaleManaTop(0, 6, 6, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 3:
                         game.ScaleManaTop(6, 5, 0, 0, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 4:
                         game.ScaleManaTop(7, 7, 0, 0 , GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 5:
                         game.ScaleManaTop(0, 0, 6, 8 , GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 6:
                         game.ScaleManaTop(0, 2, 2, 2, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 7:
                         game.ScaleManaTop(8, 8, 8, 8, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;


                     case 8:
                         game.ScaleManaTop(6, 6, 6, 6 , GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 9:
                         game.ScaleManaTop(0, 0, 6, 8, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 10:
                         game.ScaleManaTop(10, 10, 0, 10, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 11:
                         game.ScaleManaTop(16, 16, 16, 16, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 12:
                         game.ScaleManaTop(0, 0, 12, 9, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;

                     case 13:
                         game.ScaleManaTop(5, 5, 3, 3, GL, RL, YL, BL
                         , GT, RT, YT, BT);
                         break;
                 }
             }
             Name.Content = FastFight.NameCls(n, game.IdHero);
         }


         public void SetTool(Label NameLb , Button Clk,int Id)
         {         
                 TextBlock tb = new TextBlock();

                 tb.Text = SpellClass.JustFindFileTool((string)NameLb.Content, Id);
                 tb.TextAlignment = TextAlignment.Center;

                 Clk.ToolTip = tb;          
         }

        // +/- к Мане Игроку/Боту
        public void ManaValue(int g,int r, int y, int b , Boolean bot)
         {
            if(!bot)
            {
                game.greenP.Value += g;
                game.redP.Value += r;
                game.yellowP.Value += y;
                game.blueP.Value += b;             
            }
            else
            {
                game.greenPE.Value += g;
                game.redPE.Value += r;
                game.yellowPE.Value += y;
                game.bluePE.Value += b;
            }

            ValueL(!bot);
         }

    }
}
