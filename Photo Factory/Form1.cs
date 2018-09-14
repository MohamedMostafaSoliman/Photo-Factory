using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Tesseract;

namespace Photo_Factory
{
    public partial class PHFactory : Form
    {
        UserRect rect;
        RGBPixel[,] ImageMatrix;
        List<string> Images_Names = new List<string>();
        public PHFactory()
        {
            InitializeComponent();
            rect = new UserRect(new System.Drawing.Rectangle(10, 10, 100, 100));
            rect.SetPictureBox(this.Pb_crop_img);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "JPG(*.JPG)|*.jpg";
            if(SFD.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Image Saveimage = Pb_SC_shot.Image;
                Saveimage.Save(SFD.FileName);
                MessageBox.Show("Image Saved Successfully");
            }
            System.Threading.Thread.Sleep(500);
            grp_scr_shot.Visible = false;
            lbl_formload.Visible = true;
            timer1.Start();
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            grb_extract_text.Visible = false;
            lbl_how_work.Visible = false;
            grb_convert_img_pdf.Visible = false;
            grb_text_type.Visible = false;
            grb_resize_img.Visible = false;
            grb_crop_img.Visible = false;
            grb_brightness.Visible = false;
            grp_scr_shot.Visible = true;
            timer1.Stop();
            lbl_how_work.Visible = false;
            timer3.Stop();
            lbl_formload.Visible = false;
            this.Hide();
            System.Threading.Thread.Sleep(1000);
            SendKeys.Send("{PRTSC}");
            System.Drawing.Image TakenImage = Clipboard.GetImage();
            Pb_SC_shot.Image = TakenImage;
           
            Pb_SC_shot.Visible = true;
            this.Show();
            grp_scr_shot.Location = new Point(10, 65);
            grp_scr_shot.Width = 843;
            grp_scr_shot.Height = 524;
            timer2.Start();
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_formload.Location = new Point(lbl_formload.Location.X, lbl_formload.Location.Y + 15);
            if (lbl_formload.Location.Y > this.Height - 280)
            {
                lbl_formload.Location = new Point(lbl_formload.Location.X, lbl_formload.Height);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "JPG(*.JPG)|*.jpg";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bm_source = new Bitmap(Pb_resize_img.Image);
                    Bitmap ModifiedImg = new Bitmap(Convert.ToInt32(Pb_resize_img.Width), Convert.ToInt32(Pb_resize_img.Height));
                    Graphics gr_dest = Graphics.FromImage(ModifiedImg);
                    gr_dest.DrawImage(bm_source, 0, 0, ModifiedImg.Width + 1, ModifiedImg.Height + 1);
                    ModifiedImg.Save(SFD.FileName);

                    MessageBox.Show("Image Saved Successfully");
                }
                grb_resize_img.Visible = false;
                lbl_formload.Visible = true;
                timer1.Start();
            }
            catch(Exception c)
            {
                MessageBox.Show(c.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Drawing.Image openimg;
            OpenFileDialog SFD = new OpenFileDialog();
            SFD.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                openimg = System.Drawing.Image.FromFile(SFD.FileName);
                Pb_resize_img.Image = openimg;  
            }
           
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void txt_height_TextChanged(object sender, EventArgs e)
        {
        }

        private void txt_width_TextChanged(object sender, EventArgs e)
        {
        }

        private void btn_resize_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_height.Text == "" || txt_width.Text == "")
                {
                    MessageBox.Show("Width and Height should be filled");
                }
                else if (Convert.ToInt32(txt_height.Text) < 0 || Convert.ToInt32(txt_width.Text) < 0)
                {
                    MessageBox.Show("No negative value allowed");
                }
                else
                {
                    Pb_resize_img.Size = new Size(Convert.ToInt32(txt_width.Text), Convert.ToInt32(txt_height.Text));
                    Bitmap ModifiedImg = new Bitmap(Pb_resize_img.Image);
                }
            }
            catch(Exception)
            {
                MessageBox.Show("No image found to open image please click on image icon");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            timer3.Stop();
            grb_extract_text.Visible = false;
            lbl_how_work.Visible = false;
            grb_convert_img_pdf.Visible = false;
            grb_text_type.Visible = false;
            grp_scr_shot.Visible = false;
            grb_brightness.Visible = false;
            grb_crop_img.Visible = false;
            grb_resize_img.Visible = true;
            Pb_resize_img.Visible = true;
            lbl_formload.Visible = false;
            grb_resize_img.Location = new Point(10, 65);
            grb_resize_img.Width = 843;
            grb_resize_img.Height = 524;
            lbl_how_work.Text = "To resize your image first search for image using open folder icon -Bottom Right- then set image height and width then click resize then save your image using save icon -Bottom right-";
            lbl_how_work.Visible = true;
            lbl_how_work.ForeColor = Color.Green;
            timer3.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lbl_save.Location = new Point(lbl_save.Location.X - 10, lbl_save.Location.Y);
            if (lbl_save.Location.X < 75)
            {
                lbl_save.Location = new Point(lbl_save.Width , lbl_save.Location.Y);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            grb_extract_text.Visible = false;
            lbl_how_work.Visible = false;
            grb_convert_img_pdf.Visible = false;
            grb_text_type.Visible = false;
            grp_scr_shot.Visible = false;
            grb_resize_img.Visible = false;
            grb_brightness.Visible = false;

            grb_crop_img.Visible = true;
            make_selection.Enabled = true;
            grb_crop_img.Location = new Point(10, 65);
            Pb_crop_img.Width = 840;
            Pb_crop_img.Height = 450;
            grb_crop_img.Width = 843;
            grb_crop_img.Height = 524;
            lbl_how_work.Text = "To crop your image first search for image using open folder icon -Bottom Right- the reshape the red rectangle to part to crop it the click crop then save it";
            lbl_how_work.Visible = true;
            lbl_how_work.ForeColor = Color.Yellow;
            timer3.Start();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
           
           
        }

        private void Pb_crop_img_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Drawing.Image openimg;
            OpenFileDialog SFD = new OpenFileDialog();
            SFD.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                openimg = System.Drawing.Image.FromFile(SFD.FileName);
                Pb_crop_img.Image = openimg;
            }
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
           
            
        }

        private void button4_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void make_selection_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void make_selection_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void make_selection_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap OriginalImage = new Bitmap(Pb_crop_img.Image, Pb_crop_img.Width, Pb_crop_img.Height);
                Bitmap _img = new Bitmap(rect.rect.Width, rect.rect.Height);
                Graphics g = Graphics.FromImage(_img);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                //set image attributes

                g.DrawImage(OriginalImage, 0, 0, rect.rect, GraphicsUnit.Pixel);
                Pb_crop_img.Image = _img;

                Pb_crop_img.Width = _img.Width;

                Pb_crop_img.Height = _img.Height;
            }
            catch(Exception)
            {
                MessageBox.Show("No Image Found please open folder and choose image");
            }
        }

        private void Pb_crop_img_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void Pb_crop_img_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "JPG(*.JPG)|*.jpg";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Image Saveimage = Pb_crop_img.Image;
                    Saveimage.Save(SFD.FileName);
                    MessageBox.Show("Image Saved Successfully");
                }
                grb_crop_img.Visible = false;
                lbl_formload.Visible = true;
            }
            catch(Exception)
            {
                MessageBox.Show("No image found");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            timer3.Stop();
            lbl_how_work.Visible = false;
            grb_extract_text.Visible = false;
            grb_convert_img_pdf.Visible = false;
            grb_text_type.Visible = false;
            grb_brightness.Visible = true;
            grb_crop_img.Visible = false;
            grb_resize_img.Visible = false;
            grp_scr_shot.Visible = false;
            grb_brightness.Location = new Point(10, 65);
            grb_brightness.Width = 843;
            grb_brightness.Height = 524;
            lbl_how_work.Text = "to inc/dec brighness or any color component to your image first search for image then click on plus button to increase this type or to decrease click on negative button then click to save your image";
            lbl_how_work.ForeColor = Color.Fuchsia;
            timer3.Start();
            lbl_how_work.Visible = true;

            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if ((ImageMatrix[i, j].red += 5) <= 255)
                        {
                            ImageMatrix[i, j].red += 5;
                        }
                        if ((ImageMatrix[i, j].green += 5) <= 255)
                        {
                            ImageMatrix[i, j].green += 5;
                        }
                        if ((ImageMatrix[i, j].blue += 5) < 255)
                        {
                            ImageMatrix[i, j].blue += 5;
                        }
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch (Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if ((ImageMatrix[i, j].red -= 5) >= 0)
                        {
                            ImageMatrix[i, j].red -= 5;
                        }
                        if ((ImageMatrix[i, j].green -= 5) >= 0)
                        {
                            ImageMatrix[i, j].green -= 5;
                        }
                        if ((ImageMatrix[i, j].blue -= 5) >= 0)
                        {
                            ImageMatrix[i, j].blue -= 5;
                        }
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch (Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if (ImageMatrix[i, j].red < 255) ImageMatrix[i, j].red += 3;
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch (Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if (ImageMatrix[i, j].red > 0) ImageMatrix[i, j].red -= 3;
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch (Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if (ImageMatrix[i, j].red < 255) ImageMatrix[i, j].green += 3;
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch (Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if (ImageMatrix[i, j].red < 255) ImageMatrix[i, j].blue+=3;
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch(Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if (ImageMatrix[i, j].red > 0) ImageMatrix[i, j].blue -= 3;
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch (Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                int img_Height = ImageOperations.GetHeight(ImageMatrix);
                int img_Width = ImageOperations.GetWidth(ImageMatrix);
                for (int i = 0; i < img_Height; i++)
                {
                    for (int j = 0; j < img_Width; j++)
                    {
                        if (ImageMatrix[i, j].red > 0) ImageMatrix[i, j].green--;
                    }
                }
                ImageOperations.DisplayImage(ImageMatrix, Pb_brightness);
            }
            catch(Exception)
            {
                MessageBox.Show("No Image Found");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
           
        }

        private void btn__save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "JPG(*.JPG)|*.jpg";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Image Saveimage = Pb_brightness.Image;
                    Saveimage.Save(SFD.FileName);
                    MessageBox.Show("Image Saved Successfully");
                }
                grb_brightness.Visible = false;
                lbl_formload.Visible = true;
                timer1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("No image found");
            }
        }

        private void Pb_brightness_Paint(object sender, PaintEventArgs e)
        {
        }
        
        private void txt_type_text_TextChanged(object sender, EventArgs e)
        {
        }

        private void button25_Click(object sender, EventArgs e)
        {
            System.Drawing.Image openimg;
            OpenFileDialog SFD = new OpenFileDialog();
            SFD.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                openimg = System.Drawing.Image.FromFile(SFD.FileName);
                Pb_type_text.Image = openimg;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                Brush defaultcolor = Brushes.Black;
                int defaultsize = 10;
                string defaultfont = "Arial";
                if (cmb_font_text.SelectedItem != null)
                {
                    defaultfont = cmb_font_text.SelectedItem.ToString();
                }
                if (txt_size_text.Text != "")
                {
                    defaultsize = Convert.ToInt32(txt_size_text.Text);
                }
                if (cmb_color_type_text.SelectedItem != null)
                {
                    if (cmb_color_type_text.SelectedItem.ToString() == "Red")
                        defaultcolor = Brushes.Red;
                    if (cmb_color_type_text.SelectedItem.ToString() == "Yellow")
                        defaultcolor = Brushes.Yellow;
                    if (cmb_color_type_text.SelectedItem.ToString() == "White")
                        defaultcolor = Brushes.White;
                    if (cmb_color_type_text.SelectedItem.ToString() == "Blue")
                        defaultcolor = Brushes.Blue;
                    if (cmb_color_type_text.SelectedItem.ToString() == "Azure")
                        defaultcolor = Brushes.Azure;
                    if (cmb_color_type_text.SelectedItem.ToString() == "Purple")
                        defaultcolor = Brushes.Purple;
                    if (cmb_color_type_text.SelectedItem.ToString() == "Brown")
                        defaultcolor = Brushes.Brown;
                    if (cmb_color_type_text.SelectedItem.ToString() == "Fuchsia")
                        defaultcolor = Brushes.Fuchsia;
                    if (cmb_color_type_text.SelectedItem.ToString() == "Gold")
                        defaultcolor = Brushes.Gold;

                }
                Bitmap originalimage = (Bitmap)Pb_type_text.Image;
                using (Graphics graphics = Graphics.FromImage(originalimage))
                {
                    using (System.Drawing.Font ourfont = new System.Drawing.Font(defaultfont, defaultsize))
                    {
                        graphics.DrawString(txt_type_text.Text, ourfont, defaultcolor, new Point(Convert.ToInt32(txt_X.Text), Convert.ToInt32(txt_Y.Text)));
                        //graphics.DrawString(txt_type_text.Text, ourfont, defaultcolor, new Point(10, 10));

                        Pb_type_text.Image = (System.Drawing.Image)originalimage;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No Image Found");
            }

        }

        private void Pb_type_text_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button18_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer3.Stop();
            grb_convert_img_pdf.Visible = false;
            grb_brightness.Visible = false;
            grb_crop_img.Visible = false;
            grb_resize_img.Visible = false;
            grb_extract_text.Visible = false;
            grp_scr_shot.Visible = false;
            grb_text_type.Visible = true;
            timer1.Stop();
            lbl_formload.Visible = false;
            grb_text_type.Location = new Point(10, 65);
            grb_text_type.Width = 843;
            grb_text_type.Height = 524;
            lbl_how_work.Visible = true;
            lbl_how_work.Text = "to write text on your image first select image from open folder icon then write text then fill remaining finally choose the X and Y axis that text will be in it then click write finaly save image";
            lbl_how_work.ForeColor = Color.Aqua;
            timer3.Start();

        }

        private void button7_Click_2(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if(SFD.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = (Bitmap)Pb_type_text.Image;
                bmp.Save(SFD.FileName);
            }
            MessageBox.Show("image saved successfully");
            grb_text_type.Visible = false;
            timer1.Start();
            lbl_formload.Visible = true;
        }

        private void txt_type_text_TextChanged_1(object sender, EventArgs e)
        {
           
        }

        private void Pb_type_text_MouseClick(object sender, MouseEventArgs e)
        {
            txt_X.Text = Convert.ToString(e.X);
            txt_Y.Text = Convert.ToString(e.Y);
        }

        private void grb_text_type_Enter(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {

            System.Drawing.Image openimg;
            string[] imgname;
            OpenFileDialog SFD = new OpenFileDialog();
            SFD.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                Images_Names.Add(SFD.FileName.ToString());
                string g = SFD.FileName.ToString();
                imgname = g.Split('\\');
                dgv_imgs_names.Rows.Add(imgname[imgname.GetLength(0) - 1]);
                
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Document pdoc = new Document(PageSize.A4, 40f, 40f, 30f, 30f);
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "JPG(*.JPG)|*.jpg";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                string path = SFD.FileName + ".pdf";
                PdfWriter pwriter = PdfWriter.GetInstance(pdoc, new FileStream(path , FileMode.Create));
                pdoc.Open();
                for (int i = 0; i < Images_Names.Count; i++ )
                {
                    System.Drawing.Image pImage = System.Drawing.Image.FromFile(Images_Names[i]);
                    iTextSharp.text.Image Itextimage = iTextSharp.text.Image.GetInstance(pImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Itextimage.Alignment = Element.ALIGN_CENTER;
                    Itextimage.ScaleAbsoluteHeight(800);
                    Itextimage.ScaleAbsoluteWidth(600);
                    pdoc.Add(Itextimage);
                }
                MessageBox.Show("Images Saved Successfully");
                pdoc.Close();
                grb_convert_img_pdf.Visible = false;
                lbl_formload.Visible = true;
                timer1.Start();
            }
            
            
           
            
            
        }

        private void button19_Click(object sender, EventArgs e)
        {
            grb_convert_img_pdf.Visible = true ;
            grb_brightness.Visible = false;
            grb_crop_img.Visible = false;
            grb_resize_img.Visible = false;
            grb_extract_text.Visible = false;
            grp_scr_shot.Visible = false;
            grb_text_type.Visible = false;
            timer1.Stop();
            lbl_formload.Visible = false;
            grb_convert_img_pdf.Location = new Point(10, 65);
            grb_convert_img_pdf.Width = 843;
            grb_convert_img_pdf.Height = 524;
            lbl_how_work.Text = "to Collect group of images into pdf file click add image then after add all the images finaly click on convert to pdf button and save your pdf file";
            lbl_how_work.Visible = true;
            lbl_how_work.ForeColor = Color.Red;
            timer3.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            lbl_how_work.Location = new Point(lbl_how_work.Location.X - 4, lbl_how_work.Location.Y);
            if (lbl_how_work.Location.X < -900)
            {
                lbl_how_work.Location = new Point(lbl_how_work.Width, lbl_how_work.Location.Y);
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog SFD = new OpenFileDialog();
            System.Drawing.Image openimg;
            SFD.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png , *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                openimg = System.Drawing.Image.FromFile(SFD.FileName);
                Bitmap newimg = new Bitmap(openimg);   
                TesseractEngine Tessengine;
                if(cmb_selecttext.SelectedItem.ToString() == null)
                {
                    MessageBox.Show("you should select the language of the text in image");
                }
                else if(cmb_selecttext.SelectedItem.ToString() == "English")
                {
                    Tessengine = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube);
                    Page page = Tessengine.Process(newimg, PageSegMode.Auto);
                    txt_textfromimg.Text = page.GetText();
                }
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            timer3.Stop();
            lbl_how_work.Visible = false;
            grb_convert_img_pdf.Visible = false;
            grb_text_type.Visible = false;
            grb_resize_img.Visible = false;
            grb_crop_img.Visible = false;
            grb_brightness.Visible = false;
            grp_scr_shot.Visible = false;
            timer1.Stop();
            lbl_how_work.Visible = false;
            timer3.Stop();
            lbl_formload.Visible = false;
            grb_extract_text.Visible = true;
        }
    }
}
