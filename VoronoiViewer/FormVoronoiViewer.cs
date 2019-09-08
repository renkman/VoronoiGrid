using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VoronoiEngine.Elements;
using VoronoiEngine.Utilities;

namespace VoronoiViewer
{
    public partial class FormVoronoiViewer : Form
    {
        private Dictionary<string, ISiteGenerator> _siteGenerators;
        private VoronoiService _voronoiService;
        private Session _session;
        private Bitmap _canvas;

        private float _factorX;
        private float _factorY;

        private int _height;
        private int _width;

        public FormVoronoiViewer()
        {
            InitializeComponent();

            _siteGenerators = new Dictionary<string, ISiteGenerator>
            {
                {nameof(RandomSiteGenerator), new RandomSiteGenerator()},
                {nameof(EvenlySpreadSiteGenerator), new EvenlySpreadSiteGenerator()}
            };
            _voronoiService = new VoronoiService(_siteGenerators.Values.First());

            comboBoxSiteGenerators.Items.AddRange(_siteGenerators.Keys.ToArray());
            comboBoxSiteGenerators.SelectedIndex = 0;

            _session = new Session();
            _canvas = new Bitmap(
                 tabPageDiagram.ClientRectangle.Width,
                 tabPageDiagram.ClientRectangle.Height,
                 System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                       
            InitCanvas();
        }

        private void InitCanvas()
        {
            using (var graphics = Graphics.FromImage(_canvas))
            {
                graphics.Clear(Color.FromArgb(255, 255, 255, 255));
            }
        }

        private void buttonGenerateSites_Click(object sender, EventArgs e)
        {
            int width;
            if (!int.TryParse(textBoxWidth.Text, out width))
                return;

            int height;
            if (!int.TryParse(textBoxHeight.Text, out height))
                return;

            _width = width;
            _height = height;

            if (optionRandom.Checked)
            {
                int count;
                if (!int.TryParse(textBoxSites.Text, out count))
                    return;

                _session.Sites = _voronoiService.GenerateSites(width, height, count);
            }
            else
            {
                var points = dataTableBindingSource.Cast<VoronoiEngine.Elements.Point>();
                _session.Sites = points.Select(p => new Site() { Point = p }).ToList();
            }

            using (var graphics = Graphics.FromImage(_canvas))
            {
                graphics.Clear(Color.FromArgb(255, 255, 255, 255));

                var brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

                _factorX = _canvas.Width * 1f / width;
                _factorY = _canvas.Height * 1f / height;
                foreach (var site in _session.Sites)
                {
                    var x = (int)Math.Round(site.Point.XInt * _factorX);
                    var y = (int)Math.Round(_canvas.Height - (site.Point.YInt * _factorY));
                    graphics.FillRectangle(brush, x, y, (int)Math.Max(1, _factorX), (int)Math.Max(1, _factorY));
                }
            }
            tabPageDiagram.Invalidate();
        }

        private void buttonVoronoiDiagram_Click(object sender, EventArgs e)
        {
            if (_session.Sites == null)
                return;

            try
            {
                var map = _voronoiService.CreateDiagram(_height, _width, _session.Sites);

                //textBoxLog.Text = string.Join(Environment.NewLine, map.Select(g => $"{g.GetType().Name}:\t{g.Point}{(g is HalfEdge ? ",\t"+((HalfEdge)g).EndPoint : null)}").ToArray());
                textBoxLog.Text = string.Join(Environment.NewLine, map.Select(g => $"{g}").ToArray());

                using (var graphics = Graphics.FromImage(_canvas))
                {
                    var halfEdgeBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
                    var vertexBrush = new SolidBrush(Color.FromArgb(255, 255, 0, 0));

                    var pen = new Pen(halfEdgeBrush);

                    foreach (var edge in map.Where(g => g is HalfEdge).Cast<HalfEdge>())
                    {
                        DrawEdge(edge, graphics, pen);
                    }
                }
            }
            catch (Exception exception)
            {
                textBoxLog.Text += $"{Environment.NewLine}{exception.Message}{Environment.NewLine}{Environment.NewLine}{exception.Source}{Environment.NewLine}{Environment.NewLine}{exception.TargetSite}{Environment.NewLine}{exception.StackTrace}{Environment.NewLine}";
                Logger.Instance.ToFile();
            }
            tabPageDiagram.Invalidate();
        }

        private void tabPageDiagram_Paint(object sender, PaintEventArgs e)
        {
            using (var graphics = e.Graphics)
            {
                graphics.DrawImage(_canvas, 0, 0, _canvas.Width, _canvas.Height);
            }
        }

        private void DrawEdge(HalfEdge halfEdge, Graphics graphics, Pen pen)
        {
            var startX = halfEdge.Point.XInt * _factorX;
            //var startX = Math.Max(0f, halfEdge.Point.XInt * _factorX);
            var startY = _canvas.Height - halfEdge.Point.YInt * _factorY;
            //var startY = Math.Max(0f, _canvas.Height - halfEdge.Point.YInt * _factorY);
            var endX = halfEdge.EndPoint.XInt * _factorX;
            //var endX = Math.Max(0f, halfEdge.EndPoint.XInt * _factorX);
            var endY = _canvas.Height - halfEdge.EndPoint.YInt * _factorY;
            //var endY = Math.Max(0f, _canvas.Height - halfEdge.EndPoint.YInt * _factorY);

            textBoxLog.Text += $"Calculate edge with start x: {halfEdge.Point.XInt}, y: {halfEdge.Point.YInt} and end x: {halfEdge.EndPoint.XInt}, y: {halfEdge.EndPoint.YInt}{Environment.NewLine}";
            textBoxLog.Text += $"Draw edge with start x: {startX}, y: {startY} and end x: {endX}, y: {endY}{Environment.NewLine}";
            graphics.DrawLine(pen, new PointF(startX, startY), new PointF(endX, endY));
        }

        private void OptionRandom_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSites.Enabled = true;
            comboBoxSiteGenerators.Enabled = true;
            dataGridViewValues.Enabled = false;
        }

        private void OptionValues_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSites.Enabled = false;
            comboBoxSiteGenerators.Enabled = false;
            dataGridViewValues.Enabled = true;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            var file = new VoronoiFile
            {
                Height = _height,
                Width = _width,
                Sites = _session.Sites
            };

            saveFileDialog.ShowDialog();
            using (var fileStream = saveFileDialog.OpenFile())
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    var data = JsonConvert.SerializeObject(file);
                    streamWriter.Write(data);
                }
                fileStream.Close();
            }
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            VoronoiFile file;
            openFileDialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(openFileDialog.FileName))
                return;

            using (var fileStream = openFileDialog.OpenFile())
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var data = streamReader.ReadToEnd();
                    file = JsonConvert.DeserializeObject<VoronoiFile>(data);
                }
                fileStream.Close();
            }

            _height = file.Height;
            _width = file.Width;
            _session = new Session { Sites = file.Sites };

            textBoxHeight.Text = _height.ToString();
            textBoxWidth.Text = _width.ToString();
            dataTableBindingSource.DataSource = Sites.Create(file.Sites);
            optionValues.Checked = true;
            optionRandom.Checked = false;
        }

        private void ComboBoxSiteGenerators_SelectedIndexChanged(object sender, EventArgs e)
        {
            var siteGenerator = _siteGenerators[comboBoxSiteGenerators.SelectedItem.ToString()];
            _voronoiService.SwitchSiteGenerator(siteGenerator);
        }
    }
}