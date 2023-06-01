namespace App;

public partial class MainForm : Form
{
	private readonly IterativeLineDrawer _drawer;

	public MainForm()
	{
		InitializeComponent();

		_drawer = new IterativeLineDrawer(
			new SystemParameters
			{
				StartPoint = new PointD
				{
					X = (double) StartPointXInput.Value,
					Y = (double) StartPointYInput.Value
				},
				EndPoint = new PointD
				{
					X = (double) EndPointXInput.Value,
					Y = (double) EndPointYInput.Value
				},
				Step = (double) StepInput.Value
			},
			(uint) PrecisionInput.Value,
			(double) EpsilonInput.Value,
			1500
		);

		Timer.Enabled = false;
	}

	private void SimulationPanel_Paint(object sender, PaintEventArgs e)
	{
		var graphics = e.Graphics;
		graphics.ScaleTransform(2, 2);

		graphics.FillEllipse(Brushes.ForestGreen, CenteredRectangle(_drawer.Parameters.StartPoint.ToPointF(), 4, 4));
		graphics.FillEllipse(Brushes.DarkSlateGray, CenteredRectangle(_drawer.CurrentPoint.ToPointF(), 4, 4));

		var radiusBrush = new SolidBrush(Color.FromArgb(50, Color.OrangeRed));
		graphics.FillEllipse(radiusBrush, CenteredRectangle(_drawer.Parameters.EndPoint.ToPointF(), (float) _drawer.Epsilon, (float) _drawer.Epsilon));
		graphics.FillEllipse(Brushes.OrangeRed, CenteredRectangle(_drawer.Parameters.EndPoint.ToPointF(), 4, 4));
	}

	private static RectangleF CenteredRectangle(PointF center, float width, float height)
	{
		var topLeft = new PointF(center.X - width / 2, center.Y - height / 2);
		return new RectangleF(topLeft, new SizeF(width, height));
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		var status = _drawer.Next();

		Timer.Enabled = status switch
		{
			DrawingStatus.InNeighbourhood => false,
			DrawingStatus.OutOfReact => false,
			DrawingStatus.Continue => true,
			_ => throw new ArgumentOutOfRangeException()
		};

		SimulationPanel.Invalidate();
	}

	private void StartSimulationButton_Click(object sender, EventArgs e)
	{
		_drawer.InitializeState();
		Timer.Enabled = true;

		StartSimulationButton.Enabled = false;
		StartPointXInput.Enabled = false;
		StartPointYInput.Enabled = false;
		EndPointXInput.Enabled = false;
		EndPointYInput.Enabled = false;
		PrecisionInput.Enabled = false;
		EpsilonInput.Enabled = false;
		StepInput.Enabled = false;
	}

	private void StartAnalysisButton_Click(object sender, EventArgs e)
	{
		var precisions = Enumerable
			.Range(2, 99)
			.Select(Convert.ToUInt32)
			.ToArray();

		var results = Analyzer.AnalyzeEpsilonAndPrecisionConnection(
			_drawer.Parameters,
			precisions
		);

		var plot = new ScottPlot.Plot(800, 600);
		plot.AddScatter(precisions.Select(Convert.ToDouble).ToArray(), results.ToArray());
		plot.Title("Зависимость между точностью расчётов и радиусом зоны попадания в конечную точку");
		plot.XLabel("Число членов суммы в формулах");
		plot.YLabel("Достаточный радиус зоны попадания");

		plot.SaveFig("dia.png");

		MessageBox.Show(
			"Исследование проведено! Файл сохранён",
			"Исследование",
			MessageBoxButtons.OK,
			MessageBoxIcon.Information
		);
	}

	private void StopButton_Click(object sender, EventArgs e)
	{
		Timer.Enabled = false;

		StartSimulationButton.Enabled = true;
		StartPointXInput.Enabled = true;
		StartPointYInput.Enabled = true;
		EndPointXInput.Enabled = true;
		EndPointYInput.Enabled = true;
		PrecisionInput.Enabled = true;
		EpsilonInput.Enabled = true;
		StepInput.Enabled = true;
	}

	private void StartPointXInput_ValueChanged(object sender, EventArgs e)
	{
		_drawer.Parameters.StartPoint.X = (double) StartPointXInput.Value;
		SimulationPanel.Invalidate();
	}

	private void StartPointYInput_ValueChanged(object sender, EventArgs e)
	{
		_drawer.Parameters.StartPoint.Y = (double) StartPointYInput.Value;
		SimulationPanel.Invalidate();
	}

	private void EndPointXInput_ValueChanged(object sender, EventArgs e)
	{
		_drawer.Parameters.EndPoint.X = (double) EndPointXInput.Value;
		SimulationPanel.Invalidate();
	}

	private void EndPointYInput_ValueChanged(object sender, EventArgs e)
	{
		_drawer.Parameters.EndPoint.Y = (double) EndPointYInput.Value;
		SimulationPanel.Invalidate();
	}

	private void EpsilonInput_ValueChanged(object sender, EventArgs e)
	{
		_drawer.Epsilon = (double) EpsilonInput.Value;
		SimulationPanel.Invalidate();
	}

	private void PrecisionInput_ValueChanged(object sender, EventArgs e)
	{
		_drawer.Precision = (uint) PrecisionInput.Value;
	}

	private void StepInput_ValueChanged(object sender, EventArgs e)
	{
		_drawer.Parameters.Step = (double) StepInput.Value;
	}
}
