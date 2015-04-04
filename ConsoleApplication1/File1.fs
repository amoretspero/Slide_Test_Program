open System
open System.Collections
open System.Diagnostics
open System.IO
open System.Linq
open System.Text
open System.Drawing
open System.Drawing.Design
open System.Threading
open System.Windows.Forms

open File2

/// Fonts. - Must include : font name, font size, FontStyle, GraphicsUnit
let font_title = new Font("나눔손글씨 펜", 14.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_text = new Font("맑은 고딕", 10.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_label = new Font("서울남산체", 10.0f, FontStyle.Regular, GraphicsUnit.Point)

/// Forms for slide_test, test_selections. - Must include : Text, Width, Height, StartPosition, AutoScaleMode, FormBorderStyle
let form_slide_test = new Form(Width = 1280, Height = 720, StartPosition = FormStartPosition.CenterScreen, AutoScaleMode = AutoScaleMode.Font, FormBorderStyle = FormBorderStyle.FixedSingle, Text = "Slide Test")

/// Labels - Must include : Text, Width, Height, 
let label_image = new Label(Text = "Image", Width = 100, Height = 20, Location = new Point(600, 20), Font = font_label)
let label_answer = new Label(Text = "Answer :", Width = 50, Height = 20, Location = new Point(200, 650), Font = font_label)
let label_correctness = new Label(Text = "Correctness : ", Width = 100, Height = 20, Location = new Point(400, 650), Font = font_label)
let label_correctness_answer = new Label(Text = "N\A", Width = 100, Height = 20, Location = new Point(500, 650), Font = font_label)


/// Buttons - Must include : Text, Width, Height, Location, Font
let button_close = new Button(Text = "Close", Width = 70, Height = 20, Location = new Point(900, 650), Font = font_label)

/// Form control adder.
let rec form_control_adder (f : Form) (lst : Control list) =
    match lst with
    [] -> f
    | h :: t -> 
        f.Controls.Add(h)
        (form_control_adder f t)

/// Event handlers.
button_close.Click.Add(fun _ ->
    form_slide_test.Close())


/// Add controls to form.
(form_control_adder form_slide_test [label_image; label_answer; label_correctness; label_correctness_answer]) |> ignore
(form_control_adder form_slide_test [button_close]) |> ignore


form_slide_test.ShowDialog() |> ignore