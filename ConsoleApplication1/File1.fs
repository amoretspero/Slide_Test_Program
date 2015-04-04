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

/// Fonts. - Must include : font name, font size, FontStyle, GraphicsUnit
let font_title = new Font("나눔손글씨 펜", 14.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_text = new Font("맑은 고딕", 10.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_label = new Font("서울남산체", 10.0f, FontStyle.Regular, GraphicsUnit.Point)

/// Forms for slide_test, test_selections. - Must include : Text, Width, Height, StartPosition, AutoScaleMode, FormBorderStyle
let form_slide_test = new Form(Width = 1280, Height = 720, StartPosition = FormStartPosition.CenterScreen, AutoScaleMode = AutoScaleMode.Font, FormBorderStyle = FormBorderStyle.FixedSingle, Text = "Slide Test")

let 