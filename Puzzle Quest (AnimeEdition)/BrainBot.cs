using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Puzzle_Quest__AnimeEdition_
{
    class BrainBot
    {

        // Статы Монстров
        public int HpTroll = 110;

        // 

        Boolean tryTurn;
        
        
        public BrainBot(MainWindow main)
         {
            this.game = main;
            SClass = new SpellClass(game);
            SetPrioruty(2, 3, 4, 7, 7, 0, 1);
         }

        MainWindow game;
        
        SpellClass SClass;

        int[] PriorutyTag = new int[7];

        Boolean NoTurn;

        Rectangle[] BestTurn = new Rectangle[4]; // Запоминает лучшие варианты хода;

        int[] valueBlock = new int[4]; // Запоминает кол-во (лучших блоков) которые можно разрушить;

        int[] valueTag = new int[4]; // Запоминает цвет блока;

        Boolean BreakTrue;

        public Boolean NoTurns()
        {

            BotTurn(true);
            return NoTurn;
        }

        public void BotTurn(Boolean CheckingTurn)
        {
            if (!CheckingTurn)
            {
                BreakTrue = false;
                for (int i = 0; i < 4; i++)
                {
                    
                    BestTurn[i] = null;
                    valueBlock[i] = 0;
                    valueTag[i] = 0;

                }
            }

            if(CheckingTurn)
            {
                NoTurn = true;
            }
            // Запускаем поиск лучших ходов;
            if (IdBrain(game.MonsterNum , CheckingTurn) || CheckingTurn)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (i < 7)
                        {
                            try { CheckDown(i, j, CheckingTurn); }
                            catch (IndexOutOfRangeException) { }
                        }
                        if (i > 0)
                        {
                            try { CheckUp(i, j, CheckingTurn); }
                            catch (IndexOutOfRangeException) { }
                        }
                        if (j > 0)
                        {
                            try { CheckLeft(i, j, CheckingTurn); }
                            catch (IndexOutOfRangeException) { }
                        }
                        if (j < 7)
                        {
                            try { CheckRight(i, j, CheckingTurn); }
                            catch (IndexOutOfRangeException) { }
                        }
                    }
                }
                TurnBot();
            }
        }

        // Row v
        private void CheckDown(int i, int j, Boolean checkNT)
        {
            //Если 3 блока Row v 
           if(j > 1 && j < 6)
           {
               if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 2].Tag) ||
                   game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 2].Tag) || 
                   game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) ||
                   i < 5 && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 3, j].Tag)) 
                {
                   
                    // Если 4 блока Row v
                    if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 2].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) || game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 2].Tag))
                    {
                        // Если 5 блоков Row v
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 2].Tag) &&
                                    game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 2].Tag))
                        {
                            ValueBlockAndTurn(5, 0, i, j, checkNT);
                        }
                            //Если 5 блоков не найдено Row v
                        else
                        {
                            ValueBlockAndTurn(4, 0, i, j, checkNT);
                        }
                    }
                    //Если 4 блока не найдено Row v
                    else
                    {
                        ValueBlockAndTurn(3, 0, i, j, checkNT);
                    }
                }
            }
           else if(j <= 1)
           {
               if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 2].Tag) || j == 1 &&
                   game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) ||
                  i < 5 && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 3, j].Tag))
               {
                   if (j > 0)
                   {
                       // Если 4 блока Row v
                       if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) &&
                           game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 2].Tag))
                       {

                           ValueBlockAndTurn(4, 0, i, j, checkNT);
                       }
                       // Если j > 0 и не найдено 4 блока
                       else
                       {
                           ValueBlockAndTurn(3, 0, i, j, checkNT);
                       }
                   }
                   //Если j == 0 и найдено 3 блока Row v
                   else
                   {
                       ValueBlockAndTurn(3, 0, i, j, checkNT);
                   }
               }
           }
           else if(j >= 6)
           {
               if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 2].Tag) || j == 6 &&
                   game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) ||
                   i < 5 && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 3, j].Tag))
               {
                   if (j < 7)
                   {
                       // Если 4 блока Row v
                       if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 2].Tag) &&
                           game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag))
                       {

                           ValueBlockAndTurn(4, 0, i, j, checkNT);
                       }
                       // Если j < 7 и не найдено 4 блока
                       else
                       {
                           ValueBlockAndTurn(3, 0, i, j, checkNT);
                       }

                   }
                   //Если j == 7 и найдено 3 блока Row v
                   else
                   {
                       ValueBlockAndTurn(3, 0, i, j, checkNT);
                   }
               }
           }
        }

        // Row ^
        private void CheckUp(int i, int j ,Boolean checkNT)
        {
            //Если 3 блока Row ^ 
            if (j > 1 && j < 6)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 2].Tag) ||
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 2].Tag) ||
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) ||
                    i > 2 && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 3, j].Tag))
                {
                    // Если 4 блока Row ^
                    if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 2].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) || game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 2].Tag))
                    {
                        // Если 5 блоков Row ^
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 2].Tag) &&
                                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 2].Tag))
                        {
                            ValueBlockAndTurn(5, 1, i, j, checkNT);
                        }
                        //Если 5 блоков не найдено Row ^
                        else
                        {
                            ValueBlockAndTurn(4, 1, i, j, checkNT);
                        }
                    }
                    //Если 4 блока не найдено Row ^
                    else
                    {
                        ValueBlockAndTurn(3, 1, i, j, checkNT);
                    }
                }
            }
            else if (j <= 1)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 2].Tag)|| j == 1 &&
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) ||
                    i > 2 && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 3, j].Tag))
                {
                    if (j > 0)
                    {
                        // Если 4 блока Row ^
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) &&
                            game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 2].Tag))
                        {

                            ValueBlockAndTurn(4, 1, i, j, checkNT);
                        }
                        // Если j > 0 и не найдено 4 блока
                        else
                        {
                            ValueBlockAndTurn(3, 1, i, j, checkNT);
                        }
                    }
                    //Если j == 0 и найдено 3 блока Row ^
                    else
                    {
                        ValueBlockAndTurn(3, 1, i, j, checkNT);
                    }
                }
            }
            else if (j >= 6)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 2].Tag) || j == 6 &&
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) ||
                    i > 2 && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 3, j].Tag))
                {
                    if (j < 7)
                    {
                        // Если 4 блока Row ^
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 2].Tag) &&
                            game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag))
                        {

                            ValueBlockAndTurn(4, 1, i, j, checkNT);
                        }
                        // Если j < 7 и не найдено 4 блока
                        else
                        {
                            ValueBlockAndTurn(3, 1, i, j, checkNT);
                        }

                    }
                    //Если j == 7 и найдено 3 блока Row ^
                    else
                    {
                        ValueBlockAndTurn(3, 1, i, j, checkNT);
                    }
                }
            }
        }

        // Column <
        private void CheckLeft(int i, int j, Boolean checkNT)
        {
            //Если 3 блока Collumm < 
            if (i > 1 && i < 6)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j - 1].Tag) ||
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j - 1].Tag) ||
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) ||
                    j > 2 && game.Rec[i, j].Tag.Equals(game.Rec[i, j - 2].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i, j - 3].Tag))
                {
                    // Если 4 блока Collumm <
                    if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j - 1].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) || game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag))
                    {
                        // Если 5 блоков Collumm <
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j - 1].Tag) &&
                                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j - 1].Tag))
                        {
                            ValueBlockAndTurn(5, 2, i, j, checkNT);
                        }
                        //Если 5 блоков не найдено Collumm <
                        else
                        {
                            ValueBlockAndTurn(4, 2, i, j, checkNT);
                        }
                    }
                    //Если 4 блока не найдено Collumm <
                    else
                    {
                        ValueBlockAndTurn(3, 2, i, j, checkNT);
                    }
                }
            }
            else if (i <= 1)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j - 1].Tag) || i == 1 &&
                    game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) ||
                    j > 2 && game.Rec[i, j].Tag.Equals(game.Rec[i, j - 2].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i, j - 3].Tag))
                {
                    if (i > 0)
                    {
                        // Если 4 блока Collumm <
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) &&
                            game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j - 1].Tag))
                        {
                            ValueBlockAndTurn(4, 2, i, j, checkNT);

                        }
                        // Если j > 0 и не найдено 4 блока
                        else
                        {
                            ValueBlockAndTurn(3, 2, i, j, checkNT);
                        }
                    }
                    //Если j == 0 и найдено 3 блока Collumm <
                    else
                    {
                        ValueBlockAndTurn(3, 2, i, j, checkNT);
                    }
                }
            }
            else if (i >= 6)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j - 1].Tag) || i == 6 &&
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag) ||
                    j > 2 && game.Rec[i, j].Tag.Equals(game.Rec[i, j - 2].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i, j - 3].Tag))
                {
                    if (i < 7)
                    {
                        // Если 4 блока Collumm <
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j - 1].Tag) &&
                            game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j - 1].Tag))
                        {

                            ValueBlockAndTurn(4, 2, i, j, checkNT);
                        }
                        // Если j < 7 и не найдено 4 блока
                        else
                        {
                            ValueBlockAndTurn(3, 2, i, j, checkNT);
                        }

                    }
                    //Если j == 7 и найдено 3 блока Collumm <
                    else
                    {
                        ValueBlockAndTurn(3, 2, i, j, checkNT);
                    }
                }
            }
        }

        // Column >
        private void CheckRight(int i, int j, Boolean checkNT)
        {
            //Если 3 блока Collumm > 
            if (i > 1 && i < 6)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j + 1].Tag) ||
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j + 1].Tag) ||
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) ||
                    j < 5 && game.Rec[i, j].Tag.Equals(game.Rec[i, j + 2].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i, j + 3].Tag))
                {
                    // Если 4 блока Collumm >
                    if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j + 1].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) || game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) &&
                        game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag))
                    {
                        // Если 5 блоков Collumm >
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j + 1].Tag) &&
                                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j - 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j + 1].Tag))
                        {
                            ValueBlockAndTurn(5, 3, i, j, checkNT);
                        }
                        //Если 5 блоков не найдено Collumm >
                        else
                        {
                            ValueBlockAndTurn(4, 3, i, j, checkNT);
                        }
                    }
                    //Если 4 блока не найдено Collumm >
                    else
                    {
                        ValueBlockAndTurn(3, 3, i, j, checkNT);
                    }
                }
            }
            else if (i <= 1)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j + 1].Tag) || i == 1 &&
                    game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag)||
                    j < 5 && game.Rec[i, j].Tag.Equals(game.Rec[i, j + 2].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i, j + 3].Tag))
                {
                    if (i > 0)
                    {
                        // Если 4 блока Collumm >
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag) &&
                            game.Rec[i, j].Tag.Equals(game.Rec[i + 2, j + 1].Tag))
                        {

                            ValueBlockAndTurn(4, 3, i, j, checkNT);
                        }
                        // Если j > 0 и не найдено 4 блока
                        else
                        {
                            ValueBlockAndTurn(3, 3, i, j, checkNT);
                        }
                    }
                    //Если j == 0 и найдено 3 блока Collumm >
                    else
                    {
                        ValueBlockAndTurn(3, 3, i, j, checkNT);
                    }
                }
            }
            else if (i >= 6)
            {
                if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j + 1].Tag) || i == 6 &&
                    game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag)||
                    j < 5 && game.Rec[i, j].Tag.Equals(game.Rec[i, j + 2].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i, j + 3].Tag))
                {
                    if (i < 7)
                    {
                        // Если 4 блока Collumm >
                        if (game.Rec[i, j].Tag.Equals(game.Rec[i - 1, j + 1].Tag) && game.Rec[i, j].Tag.Equals(game.Rec[i - 2, j + 1].Tag) &&
                            game.Rec[i, j].Tag.Equals(game.Rec[i + 1, j + 1].Tag))
                        {
                            ValueBlockAndTurn(4, 3, i, j, checkNT);
                        }
                        // Если j < 7 и не найдено 4 блока
                        else
                        {
                            ValueBlockAndTurn(3, 3, i, j, checkNT);
                        }

                    }
                    //Если j == 7 и найдено 3 блока Collumm >
                    else
                    {
                        ValueBlockAndTurn(3, 3, i, j, checkNT);
                    }
                }
            }
        }


        private void TurnBot()
        {
            
            for (int j = 5; j >= 3; j--)
            {
                for(int i = 0; i < 4; i++)
                {
                    if(valueBlock[i] == j)
                    {
                        if(i == 0)
                        {
                            SwapRec(1, 0, i);
                        }
                        else if(i == 1)
                        {
                            SwapRec(-1, 0, i);
                        }
                        else if (i == 2)
                        {
                            SwapRec(0, -1, i);
                        }
                        else if (i == 3)
                        {
                            SwapRec(0, 1, i);
                        }
                        BreakTrue = true;
                        break;
                    }
                }
                if (BreakTrue)
                    break;
            }
            
        }

        private void SwapRec(int Numi, int Numj,int i)
        {
            
      /*      Rectangle rc = new Rectangle();

            rc.Tag = game.Rec[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])].Tag;
            game.Rec[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])].Tag = game.Rec[Grid.GetRow(BestTurn[i]) + Numi, Grid.GetColumn(BestTurn[i]) + Numj].Tag;
            game.Rec[Grid.GetRow(BestTurn[i]) + Numi, Grid.GetColumn(BestTurn[i]) + Numj].Tag = rc.Tag;

            rc.Fill = game.Rec[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])].Fill;
            game.Rec[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])].Fill = game.Rec[Grid.GetRow(BestTurn[i]) + Numi, Grid.GetColumn(BestTurn[i]) + Numj].Fill;
            game.Rec[Grid.GetRow(BestTurn[i]) + Numi, Grid.GetColumn(BestTurn[i]) + Numj].Fill = rc.Fill;
        */
           
            game.BackgroundRecHover[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])].Visibility = Visibility.Visible;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.35) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                game.MainW.IsEnabled = false;

                Point pos = game.Rec[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])].TranslatePoint(new Point(0, 0), (VisualTreeHelper.GetParent(game.MainW) as UIElement));
                Point pos2 = game.Rec[Grid.GetRow(BestTurn[i]) + Numi, Grid.GetColumn(BestTurn[i]) + Numj].TranslatePoint(new Point(0, 0), (VisualTreeHelper.GetParent(game.MainW) as UIElement));

                game.BackgroundRecHover[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])].Visibility = Visibility.Hidden;

                Counter counter = new Counter();

                counter.Recs1 = game.Rec[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])];
                counter.Recs2 = game.Rec[Grid.GetRow(BestTurn[i]) + Numi, Grid.GetColumn(BestTurn[i]) + Numj];
                counter.trueRepeat = false;
                counter.botTurn = true;
                counter.p1 = pos;
                counter.p2 = pos2;

                game.myThread = new Thread(new ParameterizedThreadStart(game.JustMoveBlock));
                game.myThread.SetApartmentState(ApartmentState.STA);
                game.myThread.Start(counter);

                //game.lightAnimation(game.Rec[Grid.GetRow(BestTurn[i]), Grid.GetColumn(BestTurn[i])], game.Rec[Grid.GetRow(BestTurn[i]) + Numi, Grid.GetColumn(BestTurn[i]) + Numj], false, true,pos,pos2);
            };
            
        }

        private void ValueBlockAndTurn(int MaxTurn, int ii, int i , int j , Boolean CheckNoTurn)
        {
            if (!CheckNoTurn)
            {
                if (valueBlock[ii] < MaxTurn)
                {
                    BestTurn[ii] = game.Rec[i, j];
                    valueBlock[ii] = MaxTurn;
                }
                else if (valueBlock[ii] == MaxTurn)
                {
                    if (PriorutyTagFocus(Int32.Parse("" + BestTurn[ii].Tag)) <= PriorutyTagFocus(Int32.Parse("" + game.Rec[i, j].Tag)))
                    {
                        BestTurn[ii] = game.Rec[i, j];
                        valueBlock[ii] = MaxTurn;
                    }
                }
            }
            if(CheckNoTurn)
                NoTurn = false;
        }

        // Устанавливаем приоритеты боту;
        public void SetPrioruty(int Green, int Red, int Yellow, int Blue, int Skull, int Exp, int Gold)
        {
            PriorutyTag[0] = Green;
            PriorutyTag[1] = Red;
            PriorutyTag[2] = Yellow;
            PriorutyTag[3] = Blue;
            PriorutyTag[4] = Skull;
            PriorutyTag[5] = Exp;
            PriorutyTag[6] = Gold;        
        }

        //Сравниваем приоритеты;
        public int PriorutyTagFocus(int Tags)
        {
            switch(Tags)
            {
                case 1 :
                    return PriorutyTag[0];
                    
                case 2:
                    return PriorutyTag[1];
                    
                case 3:
                    return PriorutyTag[2];
                    
                case 4:
                    return PriorutyTag[3];
                    
                case 5:
                    return PriorutyTag[4];
                   
                case 6:
                    return PriorutyTag[5];
                   
                case 7:
                    return PriorutyTag[6];
        
                default :
                    return 0;
            }
        }

        //Идентифицируем бота;
        public void SpellMonster(int Mon)
        {
            TextBlock tb = new TextBlock();
            tb.TextAlignment = TextAlignment.Center;
            HiddensSpell();
            // Тролль 0;
            if(Mon == 0)
            {
                AddSpellMob(game.NameSpell9, game.ClkSpell6, game.Spell1Grid1, 
                    "Исцеление троля", "Восстанавливает 7 очков здоровья", 1, 0, 0, 0, 4);
            }
            // Скелет 1;
            else if(Mon == 1)
            {
                AddSpellMob(game.NameSpell9, game.ClkSpell6, game.Spell1Grid1,
                    "Хладная хватка", "Уменьшает все вражеские\n запасы на 3.\nДобавляет +5 к зеленой Мане", 1, 0, 4, 4, 0);

                ////////////////////////////////////////

                AddSpellMob(game.NameSpell6, game.ClkSpell7, game.Spell2Grid1,
                    "Разбудить мертвых", "Наносит ущерб в размере 5 очков", 2, 10, 0, 0, 5);
            }
                //Король лич ?;
            else if(Mon == 2)
            {
                AddSpellMob(game.NameSpell9, game.ClkSpell6, game.Spell1Grid1,
                    "Смертельное облако", "Наносит ущерб, равный половине вражеского \nздоровья \nСлучайно создает череп за каждые 5 очков\nущерба (макс. лимит - 10).", 1, 30, 20, 0, 0);

                AddSpellMob(game.NameSpell6, game.ClkSpell7, game.Spell2Grid1,
                    "Обмен душами", "Обмен запасами маны с\nврагом\nВаш ход не заканчивается", 2, 6, 0, 12, 18);

                AddSpellMob(game.NameSpell7, game.ClkSpell8, game.Spell3Grid1,
                   "Разбудить мертвых", "Хз скоро", 3, 10, 0, 0, 4);
            }
        }

        private void AddSpellMob(Label NameSpell, Button ClkSpell, Grid SpellGrid, String name, String Tool, int NumSpell,
                                 int g, int r, int y, int b)
        {
            NameSpell.Visibility = Visibility.Visible;
            ClkSpell.Visibility = Visibility.Visible;
            SpellGrid.Visibility = Visibility.Visible;

            NameSpell.Content = name;

            TextBlock tb = new TextBlock();

            tb.Text = Tool;
            tb.TextAlignment = TextAlignment.Center;

            ClkSpell.ToolTip = tb;

            ValueMana(NumSpell, g, r, y, b);
        }

        //Определяем кол-во маны бота на стоимость;
        private void ValueMana(int NumSpell, int g, int r, int y, int b)
        {
            switch (NumSpell)
            {
                case 1:
                    game.ScaleManaTop(g, r, y, b, game.GreenManaLab5, game.RedManaLab5, game.YellowManaLab5, game.BlueManaLab5
                , game.GreenManaText5, game.RedManaText5, game.YellowManaText5, game.BlueManaText5);
                    break;
                case 2:
                    game.ScaleManaTop(g, r, y, b, game.GreenManaLab6, game.RedManaLab6, game.YellowManaLab6, game.BlueManaLab6
                , game.GreenManaText6, game.RedManaText6, game.YellowManaText6, game.BlueManaText6);
                    break;
                case 3:
                    game.ScaleManaTop(g, r, y, b, game.GreenManaLab7, game.RedManaLab7, game.YellowManaLab7, game.BlueManaLab7
                , game.GreenManaText7, game.RedManaText7, game.YellowManaText7, game.BlueManaText7);
                    break;
                case 4:
                    game.ScaleManaTop(g, r, y, b, game.GreenManaLab8, game.RedManaLab8, game.YellowManaLab8, game.BlueManaLab8
                , game.GreenManaText8, game.RedManaText8, game.YellowManaText8, game.BlueManaText8);
                    break;
                case 5:
                    game.ScaleManaTop(g, r, y, b, game.GreenManaLab9, game.RedManaLab9, game.YellowManaLab9, game.BlueManaLab9
                , game.GreenManaText9, game.RedManaText9, game.YellowManaText9, game.BlueManaText9);
                    break;
            }
        }

        // Прячем все способности бота;
        private void HiddensSpell()
        {
            game.NameSpell10.Visibility = Visibility.Hidden;
            game.Spell5Grid1.Visibility = Visibility.Hidden;
            game.ClkSpell10.Visibility = Visibility.Hidden;

            game.NameSpell9.Visibility = Visibility.Hidden;
            game.Spell4Grid1.Visibility = Visibility.Hidden;
            game.ClkSpell9.Visibility = Visibility.Hidden;

            game.NameSpell8.Visibility = Visibility.Hidden;
            game.Spell3Grid1.Visibility = Visibility.Hidden;
            game.ClkSpell8.Visibility = Visibility.Hidden;

            game.NameSpell7.Visibility = Visibility.Hidden;
            game.Spell2Grid1.Visibility = Visibility.Hidden;
            game.ClkSpell7.Visibility = Visibility.Hidden;

            game.NameSpell6.Visibility = Visibility.Hidden;
            game.Spell1Grid1.Visibility = Visibility.Hidden;
            game.ClkSpell6.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Распознает бота по его id, и подключает в игру, способности бота. 
        /// </summary>
        /// <param name="idMon">id бота</param>
        /// <param name="checkTurn">Ход игрока</param>
        /// <returns>Возвращает bool, если есть возможность продолжить ход, после использования способности</returns>
        private Boolean IdBrain(int idMon, Boolean checkTurn)
        {
            if (!checkTurn)
            {
                Boolean bl;
                switch (idMon)
                {
                    //Тролль;
                    case 0:
                        bl = true;
                        BrainTroll();
                        return bl;

                    //Скелет;
                    case 1:
                        bl = BrainSkelet();
                        return bl;
                    //Лич;
                    case 2:
                        bl = BrainKingLich();
                        return bl;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Способности Короля лича.
        /// </summary>
        /// <returns></returns>
        private bool BrainKingLich()
        {
            Boolean trUseSkill = false;
            
            if (CheckManaCostSpell(1))
            {
                if (game.heroP.Maximum / 4 <= game.heroP.Value)
                {                                  
                    for (int i = 0; i < (int)(game.heroP.Value / 2) / 5; i++)
                    {
                        int ai = SClass.randomGenerator.Next(0, 8); // Служит для рандома блока в который будет засунут череп.
                        int bj = SClass.randomGenerator.Next(0,8); // Служит для рандома блока в который будет засунут череп.

                        MessageBox.Show((i + 1).ToString());
                        if (!game.Rec[ai, bj].Tag.Equals("5"))
                        {
                            game.Rec[ai, bj].Fill = game.Brushes_Rand(5, game.Rec[ai, bj]);
                        }
                        else
                            i--;
                    }
                    SClass.ManaValue(-30, -20, 0, 0, true);
                    game.Damage((int)game.heroP.Value / 2, false);

                    game.MainW.IsEnabled = true;
                    game.CheckToDestroidBlock(game.TurnHeroAndEnemy);

                    trUseSkill = true;
                    return false; 
                }
            }
            
            if (CheckManaCostSpell(2) && !trUseSkill)
            {
                if (game.greenP.Value >= game.greenPE.Value && (game.redP.Value >= game.redPE.Value ||
                    game.yellowP.Value >= game.yellowPE.Value && game.blueP.Value >= game.bluePE.Value))
                {
                    MessageBox.Show("eee");
                    SClass.ManaValue(-6, 0, -12, -18, true);
                    MessageBox.Show("хУХ");


                    //Меняет местами ману героя и бота;
                    double[] a = new double[4];
                    a[0] = game.greenP.Value;
                    a[1] = game.redP.Value;
                    a[2] = game.yellowP.Value;
                    a[3] = game.blueP.Value;

                    game.greenP.Value = game.greenPE.Value;
                    game.redP.Value = game.redPE.Value;
                    game.yellowP.Value = game.yellowPE.Value;
                    game.blueP.Value = game.bluePE.Value;

                    game.greenPE.Value = a[0];
                    game.redPE.Value = a[1];
                    game.yellowPE.Value = a[2];
                    game.bluePE.Value = a[3];

                    SClass.ValueL(true);
                    SClass.ValueL(false);

                    return true;
                }
            }

            if(CheckManaCostSpell(3) && !trUseSkill && game.enemyP.Maximum / 2 >= game.enemyP.Value )
            {
                game.Damage(5, true);
                game.enemyP.Value += 10;
                SClass.ManaValue(-10, 0, 0, -4, true);

                return true;
            }
            return true;
        }

        private Boolean BrainSkelet()
        {
            if (CheckManaCostSpell(1))
            {
                int temp = 0;
                if (Int32.Parse("" + game.redL.Content) >= 3)
                    temp++;
                if (Int32.Parse("" + game.greenL.Content) >= 3)
                    temp++;
                if (Int32.Parse("" + game.yellowL.Content) >= 3)
                    temp++;
                if (Int32.Parse("" + game.blueL.Content) >= 3)
                    temp++;

                if(temp >= 3)
                {

                    MessageBox.Show("-Мана");
                    SClass.ManaValue(5, -4, -4, 0, true);
                    SClass.ManaValue(-3, -3, -3, -3, false);
                                      
                }

                return true;
            }
            if (CheckManaCostSpell(2))
            {

                MessageBox.Show("ПО ЕБЛУ НА");
                SClass.ManaValue(-10, 0, 0, -5, true);

                game.Damage(5, false);

                game.MainW.IsEnabled = true;
                game.TurnHeroAndEnemy = game.SwapBool(game.TurnHeroAndEnemy);
                return false;
            }

            return true;
        }

        private void BrainTroll()
        {
            if (HpTroll - 7 >= game.enemyP.Value && CheckManaCostSpell(1) && game.bluePE.Value >= 5)
            {
                game.bluePE.Value -= 4;
                game.blueLE.Content = "" + (game.bluePE.Value - 1);

                game.enemyP.Value += 7;
                game.enemyL.Content = "" + game.enemyP.Value;

                MessageBox.Show("ХИИИИИИИИИИИИЛЛЛЛ");

                BrainTroll();

            }
        }

        /// <summary>
        /// Сравнивает количество маны на способность с текущей маной
        /// </summary>
        /// <param name="n">Номер способности</param>
        /// <returns>Возвращает bool хватает ли маны или нет</returns>
        public bool CheckManaCostSpell(int n)
        {
            switch(n)
            {
                case 1 :     
            
                        if(game.GreenManaText5.Foreground == Brushes.Black &&
                           game.RedManaText5.Foreground == Brushes.Black &&
                           game.YellowManaText5.Foreground == Brushes.Black && 
                           game.BlueManaText5.Foreground == Brushes.Black)
                        {
                            return true;
                        }
                        break;

                case 2 : 

                    if(game.GreenManaText6.Foreground == Brushes.Black &&
                           game.RedManaText6.Foreground == Brushes.Black &&
                           game.YellowManaText6.Foreground == Brushes.Black && 
                           game.BlueManaText6.Foreground == Brushes.Black)
                        {
                            return true;
                        }
                        break;

                case 3:

                    if(game.GreenManaText7.Foreground == Brushes.Black &&
                           game.RedManaText7.Foreground == Brushes.Black &&
                           game.YellowManaText7.Foreground == Brushes.Black && 
                           game.BlueManaText7.Foreground == Brushes.Black)
                        {
                            return true;
                        }
                        break;

                case 4:

                        if (game.GreenManaText8.Foreground == Brushes.Black &&
                               game.RedManaText8.Foreground == Brushes.Black &&
                               game.YellowManaText8.Foreground == Brushes.Black &&
                               game.BlueManaText8.Foreground == Brushes.Black)
                        {
                            return true;
                        }
                        break;

                case 5:

                        if (game.GreenManaText9.Foreground == Brushes.Black &&
                               game.RedManaText9.Foreground == Brushes.Black &&
                               game.YellowManaText9.Foreground == Brushes.Black &&
                               game.BlueManaText9.Foreground == Brushes.Black)
                        {
                            return true;
                        }
                        break;
            }

            return false;
        }

        // Сравнивает кол-во вашей маны с маной которую надо затратить чтобы использовать спосбоность;
        public void CheckValueMana(Label GT, Label RT, Label YT, Label BT)
        {
            int g, r, y, b;

            g = Int32.Parse("" + game.greenLE.Content);
            r = Int32.Parse("" + game.redLE.Content);
            y = Int32.Parse("" + game.yellowLE.Content);
            b = Int32.Parse("" + game.blueLE.Content);

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
    
    }
}
