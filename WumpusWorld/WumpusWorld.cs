using System;
using System.Drawing;
using System.Windows.Forms;


namespace WumpusWorld
{
    public partial class WumpusWorld : Form
    {
        Random rand = new Random();
        public int[] currentPos = new int[2];
        public int[] prevPos = new int[2];
        public Node[,] board = new Node[4, 4];
        public int dir = 3; //Direction 0 = Up; 1 = Down; 2 = Left; 3 = Right
        public bool arrowFired = false;
        
        Node prevNode;

        public WumpusWorld()
        {
            InitializeComponent();
            createBoard();
            linkNodes(board);
            currentPos[0] = board[3,0].posY;
            currentPos[1] = board[3,0].posX;
            board[3, 0].myButton.Image = Image.FromFile(@"../../Sara_Right.png");
            dir = 3;
            board[3, 0].getStatus(board[3, 0], textBox1, textBox2, textBox3);
            prevNode = board[3, 0];
        }

        
        private void wasClicked(Node node)
        {
            textBox4.Visible = false;
            //Perform only if the Node clicked is Right, Left, Below or Above the clicked node
            if ((currentPos[0] == node.posY && (currentPos[1] == node.posX + 1 || currentPos[1] == node.posX - 1)) || //Check Right / Left
                (currentPos[1] == node.posX && (currentPos[0] == node.posY + 1 || currentPos[0] == node.posY - 1))) // Check Down / Up
            {
                if (node.checkNode(node, textBox4))//Check if the player died or won, if so reset the board.
                {
                    resetBoard();
                    return;
                }
                node.myButton.Image = prevNode.myButton.Image;
                prevNode.myButton.Image = null;
                prevNode.myButton.BackgroundImage = null;
                prevNode.myButton.BackColor = Color.Black;
                prevNode = node;
                currentPos[0] = node.posY;
                currentPos[1] = node.posX;
                node.myButton.BackColor = Color.White;
                node.getStatus(node, textBox1, textBox2, textBox3);//Gather status from adjacent nodes.              
            }
            else 
            {
                prevNode.myButton.Select();
            }
        }

        private void shootArrow()
        {
            if (arrowFired == false)
            {
                arrowFired = true;
                switch (dir)
                {
                    case 0://Check nodes above
                        for (int i = currentPos[0]; i >= 0; i--)
                        {
                            if (board[i, currentPos[1]].wumpusAlive == true)
                            {
                                board[i, currentPos[1]].wumpusAlive = false;
                                textBox5.Visible = true;
                            }
                        }
                        return;
                    case 1://Check nodes below
                        for (int i = currentPos[0]; i < board.GetLength(0); i++)
                        {
                            if (board[i, currentPos[1]].wumpusAlive == true)
                            {
                                board[i, currentPos[1]].wumpusAlive = false;
                                textBox5.Visible = true;
                            }
                        }
                        return;
                    case 2://Check nodes left
                        for (int i = currentPos[1]; i >= 0; i--)
                        {
                            if (board[currentPos[0], i].wumpusAlive == true)
                            {
                                board[currentPos[0], i].wumpusAlive = false;
                                textBox5.Visible = true;
                            }

                        }
                        return;
                    case 3://Check nodes right
                        for (int i = currentPos[1]; i < board.GetLength(1); i++)
                        {
                            if (board[currentPos[0], i].wumpusAlive == true)
                            {
                                board[currentPos[0], i].wumpusAlive = false;
                                textBox5.Visible = true;
                            }

                        }
                        return;
                } 
            }
            
            
        }

        private void createBoard()
        {
            bool wumpusPlaced = false;
            bool goldPlaced = false;
            Node node0 = new Node(button0, 3, 0);
            board[3, 0] = node0;
            Node node1 = new Node(button1, 3, 1);
            board[3, 1] = node1;
            Node node2 = new Node(button2, 3, 2);
            board[3, 2] = node2;
            Node node3 = new Node(button3, 3, 3);
            board[3, 3] = node3;
            Node node4 = new Node(button4, 2, 0);
            board[2, 0] = node4;
            Node node5 = new Node(button5, 2, 1);
            board[2, 1] = node5;
            Node node6 = new Node(button6, 2, 2);
            board[2, 2] = node6;
            Node node7 = new Node(button7, 2, 3);
            board[2, 3] = node7;
            Node node8 = new Node(button8, 1, 0);
            board[1, 0] = node8;
            Node node9 = new Node(button9, 1, 1);
            board[1, 1] = node9;
            Node node10 = new Node(button10, 1, 2);
            board[1, 2] = node10;
            Node node11 = new Node(button11, 1, 3);
            board[1, 3] = node11;
            Node node12 = new Node(button12, 0, 0);
            board[0, 0] = node12;
            Node node13 = new Node(button13, 0, 1);
            board[0, 1] = node13;
            Node node14 = new Node(button14, 0, 2);
            board[0, 2] = node14;
            Node node15 = new Node(button15, 0, 3);
            board[0, 3] = node15;
            foreach (Node node in board)
            {
                if (node.posX == 0 && node.posY == 3) //Ensure start node has no pit or wumpus.
                {
                    currentPos[0] = board[3, 0].posY;
                    currentPos[1] = board[3, 0].posX;
                    node.myButton.BackColor = Color.White;
                }
                else
                {
                    if (rand.Next(5) == 0 && wumpusPlaced == false && node.hasPit == false && node.hasWumpus == false)
                    {
                        node.hasWumpus = true;
                        node.wumpusAlive = true;
                        wumpusPlaced = true;
                    }
                    else if (rand.Next(5) == 0 && goldPlaced == false && node.hasPit == false && node.hasWumpus == false)
                    {
                        node.hasGold = true;
                        goldPlaced = true;
                    }
                    else if (rand.Next(5) == 0 && node.hasGold == false && node.hasWumpus == false)
                    {
                        node.hasPit = true;
                    }
                }

            }
            if (wumpusPlaced == false) //Ensure wumpus is placed on the board
            {
                foreach (Node node in board)
                {
                    if (node.hasPit == false && node.hasGold == false)
                    {
                        node.hasWumpus = true;
                        node.wumpusAlive = true;
                        wumpusPlaced = true;
                        break;
                    }
                }
            }
            if (goldPlaced == false) //Ensure gold is placed on the board
            {
                foreach (Node node in board)
                {
                    if (node.hasPit == false && node.hasWumpus == false)
                    {
                        node.hasGold = true;
                        goldPlaced = true;
                        break;
                    }
                }
            }
            
        }

        private void resetBoard()
        {
            bool wumpusPlaced = false;
            bool goldPlaced = false;
            textBox5.Visible = false;
            dir = 3;
            button0.Select();
            arrowFired = false;
            //Clear the board of previous objects.
            foreach (Node node in board)
            {
                node.hasPit = false;
                node.hasWumpus = false;
                node.wumpusAlive = false;
                node.hasGold = false;
                node.myButton.BackColor = Color.Black;
                node.myButton.BackgroundImage = null;
                node.myButton.Image = null;
                node.visited = false;
                node.isSafe = false;
            }

            //Reset Wumpus and place traps.
            foreach (Node node in board)
            {
                if (node.posX == 0 && node.posY == 3) //Ensure start node has no pit or wumpus.
                {
                    currentPos[0] = board[3, 0].posY;
                    currentPos[1] = board[3, 0].posX;
                    node.myButton.BackColor = Color.White;
                    node.myButton.Image = Image.FromFile(@"../../Sara_Right.png");
                    prevNode = node; //Reassign prevNode to origin
                }
                else
                {

                    if (rand.Next(5) == 0 && goldPlaced == false && node.hasPit == false && node.hasWumpus == false)
                    {
                        node.hasGold = true;
                        goldPlaced = true;
                    }
                    else if (rand.Next(5) == 0 && wumpusPlaced == false && node.hasPit == false && node.hasWumpus == false && node.hasGold == false)
                    {
                        node.hasWumpus = true;
                        node.wumpusAlive = true;
                        wumpusPlaced = true;
                    }
                    else if (rand.Next(5) == 0 && node.hasGold == false && node.hasWumpus == false)
                    {
                        node.hasPit = true;
                    }
                }

            }
            if (wumpusPlaced == false) //Ensure wumpus is placed on the board
            {
                foreach (Node node in board)
                {
                    if (node.hasPit == false && node.hasGold == false)
                    {
                        node.hasWumpus = true;
                        node.wumpusAlive = true;
                        wumpusPlaced = true;
                        break;
                    }
                }
            }
            if (goldPlaced == false) //Ensure gold is placed on the board
            {
                foreach (Node node in board)
                {
                    if (node.hasPit == false && node.hasWumpus == false)
                    {
                        node.hasGold = true;
                        goldPlaced = true;
                        break;
                    }
                }
            }
            
            textBox4.Visible = true;
            board[3, 0].getStatus(board[3, 0], textBox1, textBox2, textBox3); //Grab new status from start.
        }

        //Link Adjacent Nodes Together
        public void linkNodes(Node[,] board)
        {
            foreach (Node node in board)
            {
                foreach (Node linkNode in board)
                {
                    if (node.posX == linkNode.posX && node.posY - 1 == linkNode.posY)
                    {
                        node.Up = linkNode;
                    }
                    else if (node.posX == linkNode.posX && node.posY + 1 == linkNode.posY)
                    {
                        node.Down = linkNode;
                    }
                    else if (node.posY == linkNode.posY && node.posX - 1 == linkNode.posX)
                    {
                        node.Left = linkNode;
                    }
                    else if (node.posY == linkNode.posY && node.posX + 1 == linkNode.posX)
                    {
                        node.Right = linkNode;
                    }
                }               
            }
        }

        //Handle Button Clicks, All buttons send their corresponding board node to wasClicked();
        private void button0_Click(object sender, EventArgs e)
        {
            wasClicked(board[3,0]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wasClicked(board[3,1]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wasClicked(board[3,2]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wasClicked(board[3,3]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            wasClicked(board[2,0]);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            wasClicked(board[2,1]);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            wasClicked(board[2,2]);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            wasClicked(board[2,3]);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            wasClicked(board[1,0]);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            wasClicked(board[1,1]);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            wasClicked(board[1,2]);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            wasClicked(board[1,3]);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            wasClicked(board[0,0]);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            wasClicked(board[0,1]);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            wasClicked(board[0,2]);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            wasClicked(board[0,3]);
        }


        //Handle Direction Keys With Each Button Focus
        private void button0_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button0.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button0.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button0.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button0.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button1.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button1.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button1.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button1.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button2.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button2.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button2.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button2.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button3.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button3.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button3.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button3.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button4.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button4.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button4.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button4.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button5.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button5.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button5.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button5.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button6.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button6.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button6.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button6.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button7.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button7.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button7.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button7.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button8.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button8.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button8.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button8.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button9.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button9.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button9.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button9.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button10.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button10.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button10.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button10.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button11.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button11.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button11.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button11.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button12.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button12.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button12.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button12.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button13.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button13.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button13.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button13.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button14.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button14.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button14.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button14.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

        private void button15_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                button15.Image = Image.FromFile(@"../../Sara_Up.png");
                dir = 0;
            }
            else if (e.KeyChar == 's')
            {
                button15.Image = Image.FromFile(@"../../Sara_Down.png");
                dir = 1;
            }
            else if (e.KeyChar == 'a')
            {
                button15.Image = Image.FromFile(@"../../Sara_Left.png");
                dir = 2;
            }
            else if (e.KeyChar == 'd')
            {
                button15.Image = Image.FromFile(@"../../Sara_Right.png");
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                shootArrow();
            }
        }

      

        

        

    }
}
