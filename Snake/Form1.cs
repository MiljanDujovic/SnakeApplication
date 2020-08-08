using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake {
	public partial class Form1 : Form {

		private List<Circle> Snake = new List<Circle>();
		private Circle food = new Circle();
		public Form1() {
			InitializeComponent();
			StartGame();
			GenerateFood();
			gameTimer.Tick += UpdateScreen;
			gameTimer.Start();
		}
		private void StartGame() {
			
			new Settings();
			// Console.Clear();
			Circle head = new Circle();
			head.X = 5;
			head.Y = 5;
			SolidBrush myBrush = new SolidBrush(Color.Red);

			Graphics canvas;
			canvas = CreateGraphics();
			canvas.FillEllipse(myBrush, 100, 100, 600, 700);
			Snake.Add(head);
			lblScore.Text = Settings.Score.ToString();
		}
		Random rnd = new Random();
		private void GenerateFood() {
			food.X = rnd.Next(0, pbCanvas.Size.Width - 5);
			food.Y = rnd.Next(0, pbCanvas.Size.Height - 5);
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) {
			Input.ChangeState(e.KeyCode, true);
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e) {
			Input.ChangeState(e.KeyCode, false);
		}
		private void UpdateScreen(object sender, EventArgs e) {

			if (Input.KeyPresed(Keys.Enter)) {
				StartGame();
			}
			else {
				if (Input.KeyPresed(Keys.Right) && Settings.direction != Direction.Left) {
					Settings.direction = Direction.Right;
				}
				else if (Input.KeyPresed(Keys.Left) && Settings.direction != Direction.Right) {
					Settings.direction = Direction.Left;
				}
				else if (Input.KeyPresed(Keys.Down) && Settings.direction != Direction.Up) {
					Settings.direction = Direction.Down;
				}
				else if (Input.KeyPresed(Keys.Up) && Settings.direction != Direction.Down) {
					Settings.direction = Direction.Up;
				}
				MovePlayer();
			}
			pbCanvas.Invalidate();

		}

		private void pbCanvas_Paint(object sender, PaintEventArgs e) {
			Graphics canvas = e.Graphics;
			if (!Settings.GameOver) {
				Brush snakeColour;
				for (int i = 0; i < Snake.Count - 1; i++) {
					if (i == 0) {

						snakeColour = Brushes.Black;
					}
					else {
						snakeColour = Brushes.Green;
					}
					canvas.FillEllipse(snakeColour, new Rectangle(Snake[i].X, Snake[i].Y, Settings.Width, Settings.Height));
					canvas.FillEllipse(Brushes.Red, new Rectangle(food.X, food.Y, Settings.Width, Settings.Height));

				}
			}
		}
		private void Eat() {
			Circle food = new Circle();
			food.X = this.food.X;
			food.Y = this.food.Y;
			Snake.Add(food);
			Settings.Score += Settings.Points;
			lblScore.Text = Settings.Score.ToString();
			GenerateFood();
		}
		private void MovePlayer() {
			for (int i = 0; i < Snake.Count - 1; i++) {
				if (i == 0) {
					switch (Settings.direction) {
						case Direction.Up:
							Snake[0].Y -= Settings.Speed;
							break;
						case Direction.Down:
							Snake[0].Y += Settings.Speed;
							break;
						case Direction.Left:
							Snake[0].X -= Settings.Speed;
							break;
						case Direction.Right:
							Snake[0].X += Settings.Speed;
							break;
						default:
							break;
					}
					if (Snake[0].X < 0 || Snake[0].Y < 0 || Snake[0].X > pbCanvas.Size.Width || Snake[0].Y > pbCanvas.Size.Height) {
						Settings.GameOver = true;
					}
					if (Difference(Snake[0].X, food.X) && Difference(Snake[0].Y, food.Y)) {
						Eat();

					}
				}
				else {
					switch (Settings.direction) {
						case Direction.Up:
							Snake[i].X = Snake[0].X;
							Snake[i].Y = Snake[0].Y + i * 10;
							break;
						case Direction.Down:
							Snake[i].X = Snake[0].X;
							Snake[i].Y = Snake[0].Y  - i*10;
							break;
						case Direction.Left:
							Snake[i].Y = Snake[0].Y;
							Snake[i].X = Snake[0].X  + i * 10;
							break;
						case Direction.Right:
							Snake[i].Y = Snake[0].Y;
							Snake[i].X = Snake[0].X  - i * 10;
							break;
						default:
							break;
					}
				}
			}
		}
		private bool Difference(int i, int j) {
			if (((i - j) < 9 && (i - j) >= 0) || ((j - i) < 9 && (j - i) >= 0)) {
				return true;
			}
			return false;
		}
	}
}
