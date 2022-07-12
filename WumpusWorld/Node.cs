using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WumpusWorld
{
    public class Node : Form
    {

        public Button myButton;
        public Node Up;
        public Node Down;
        public Node Left;
        public Node Right;
        public int value = 0; //Used by the logic bot.
        public int posX, posY;
        public bool visited = false;
        public bool isSafe = false;
        public double pitProb = 0.0;
        public bool hasBreeze = false;
        public bool hasStench = false;
        public bool hasGlitter = false;

        public bool hasPit, hasWumpus, hasGold, closePit, closeGold, closeWumpus, wumpusAlive;
        

        public Node(Button button, int PosY, int PosX)
        {
            myButton = button;            
            posX = PosX;
            posY = PosY;
            hasGold = false;
            hasWumpus = false;
            hasPit = false;
        }

        public void getStatus(Node node, TextBox textBox1, TextBox textBox2, TextBox textBox3)
        {

            if ((node.Up != null && node.Up.hasGold) || (node.Down != null && node.Down.hasGold) || (node.Left != null && node.Left.hasGold) || (node.Right != null && node.Right.hasGold))
            {
                node.myButton.BackColor = Color.Yellow;
                closeGold = true;
                textBox1.BackColor = Color.Yellow;
                textBox1.Text = "Glitter";
            }
            else
            {
                textBox1.Text = "";
                textBox1.BackColor = Color.White;
                closeGold = false;
            }
            if ((node.Up != null && node.Up.hasPit) || (node.Down != null && node.Down.hasPit) || (node.Left != null && node.Left.hasPit) || (node.Right != null && node.Right.hasPit))
            {
                node.myButton.BackColor = Color.Blue;
                closePit = true;
                textBox2.BackColor = Color.Blue;
                textBox2.Text = "Breeze";
            }
            else
            {
                textBox2.Text = "";
                textBox2.BackColor = Color.White;
                closePit = false;
            }
            if ((node.Up != null && node.Up.hasWumpus) || (node.Down != null && node.Down.hasWumpus) || (node.Left != null && node.Left.hasWumpus) || (node.Right != null && node.Right.hasWumpus))
            {
                node.myButton.BackColor = Color.Green;
                closeWumpus = true;
                textBox3.BackColor = Color.Green;
                textBox3.Text = "Stench";
            }
            else
            {
                textBox3.Text = "";
                textBox3.BackColor = Color.White;
                closeWumpus = false;
            }
            if (closePit && closeGold)
            {
                node.myButton.BackgroundImage = Image.FromFile(@"../../Gold_Pit.png");
                textBox1.Text = "Glitter";
                textBox1.BackColor = Color.Yellow;
                textBox2.Text = "Breeze";
                textBox2.BackColor = Color.Blue;
            }
            if (closeWumpus && closeGold)
            {
                node.myButton.BackgroundImage = Image.FromFile(@"../../Gold_Wumpus.png");
                textBox1.Text = "Glitter";
                textBox1.BackColor = Color.Yellow;
                textBox3.Text = "Stench";
                textBox3.BackColor = Color.Green;
            }
            if (closeWumpus && closePit)
            {
                node.myButton.BackgroundImage = Image.FromFile(@"../../Pit_Wumpus.png");
                textBox2.Text = "Breeze";
                textBox2.BackColor = Color.Blue;
                textBox3.Text = "Stench";
                textBox3.BackColor = Color.Green;
            }
            if (closeGold && closePit && closeWumpus)
            {
                node.myButton.BackgroundImage = Image.FromFile(@"../../Gold_Pit_Wumpus.png");
                textBox1.Text = "Glitter";
                textBox1.BackColor = Color.Yellow;
                textBox2.Text = "Breeze";
                textBox2.BackColor = Color.Blue;
                textBox3.Text = "Stench";
                textBox3.BackColor = Color.Green;
            }
            if (node.wumpusAlive == false && node.hasWumpus == true)
            {
                node.myButton.BackgroundImage = Image.FromFile(@"../../Dead_Wumpus.png");
            }
        }

        public bool checkNode(Node node, TextBox textBox4)
        {
            if (node.hasPit || (node.hasWumpus && node.wumpusAlive))
            {
                textBox4.Text = "YOU HAVE DIED!!";
                textBox4.BackColor = Color.Red;
                node.myButton.Image = null;
                return true;
            }
            else if (node.hasGold)
            {
                textBox4.Text = "YOU FOUND THE GOLD!!";
                textBox4.BackColor = Color.Gold;
                node.myButton.Image = null;
                return true;
            }
            else return false;
        }
    }
}
