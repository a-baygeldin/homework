(*
    A. Baygeldin (c) 2014
    GUI for Intersect

    Говнокод в массы! 
    Побит рекорд: 21 WTF per. minute
*)

module Main

open System
open System.Globalization
open System.Windows.Forms
open System.Windows.Shapes
open System.Windows.Media
open System.Windows
open System.Text.RegularExpressions
open Xceed.Wpf.Toolkit
open Intersect
open FSharpx


let mutable intersection = NoPoint

let mainWindow() =
    let window = new XAML<"Layout.xaml">()
    let parse (string:string) = Double.Parse(string, CultureInfo.InvariantCulture)
    let enableRegexp (textbox:Controls.TextBox) = 
        (fun (e:Input.TextCompositionEventArgs) ->
           if not (Regex.IsMatch(e.Text, "^[\d\+\-\.]$")) then e.Handled <- true)
        |> textbox.PreviewTextInput.Add
    let drawPoint() =
        let point = new Ellipse()
        let X = window.PointX.Text |> parse
        let Y = window.PointY.Text |> parse
        point.Margin <- new Thickness(X, Y, 0.0, 0.0)
        point.Width <- 3.0
        point.Height <- 3.0
        point.StrokeThickness <- 2.0
        point.Stroke <- Brushes.Black
        window.Canvas.Children.Add(point) |> ignore
        match intersection with
        | NoPoint -> intersection <- Point(X, Y)
        | _ -> intersection <- Intersect(Point(X,Y), intersection)
    let drawLine() =
        let line = new Line()
        let B = window.LineB.Text |> parse
        let K = window.LineK.Text |> parse
        line.X1 <- -250.0
        line.X2 <- 250.0
        line.Y1 <- K * line.X1 + B
        line.Y2 <- K * line.X2 + B
        line.StrokeThickness <- 2.0
        line.Stroke <- Brushes.Black
        window.Canvas.Children.Add(line) |> ignore
        match intersection with
        | NoPoint -> intersection <- Line(B, K)
        | _ -> intersection <- Intersect(Line(B, K), intersection)
    let drawVerticalLine() =
        let line = new Line()
        let X = window.VerticalLineX.Text |> parse
        line.X1 <- X
        line.X2 <- X
        line.Y1 <- -200.0
        line.Y2 <- 200.0
        line.StrokeThickness <- 2.0
        line.Stroke <- Brushes.Black
        window.Canvas.Children.Add(line) |> ignore
        match intersection with
        | NoPoint -> intersection <- VerticalLine(X)
        | _ -> intersection <- Intersect(VerticalLine(X), intersection)
    let drawLineSegment() =
        let line = new Line()
        let X1 = window.LineSegmentX1.Text |> parse
        let X2 = window.LineSegmentX2.Text |> parse
        let Y1 = window.LineSegmentY1.Text |> parse
        let Y2 = window.LineSegmentY2.Text |> parse
        line.X1 <- X1
        line.X2 <- X2
        line.Y1 <- Y1
        line.Y2 <- Y2
        line.StrokeThickness <- 2.0
        line.Stroke <- Brushes.Black
        window.Canvas.Children.Add(line) |> ignore
        match intersection with
        | NoPoint -> intersection <- LineSegment((X1, Y1), (X2, Y2))
        | _ -> intersection <- Intersect(LineSegment((X1, Y1), (X2, Y2)), intersection)
    let intersect() =
        match intersection with
        | Intersect(_, _) -> window.Result.Text <- (getIntersect intersection).ToString()  
        | _ -> window.Result.Text <- intersection.ToString()
    let textboxes = [| window.PointX; window.PointY; window.LineB; window.LineK; 
        window.VerticalLineX; window.LineSegmentX1; window.LineSegmentX2;
        window.LineSegmentY1; window.LineSegmentY2 |]
    for elem in textboxes do elem |> enableRegexp
    window.AddPoint.Click.Add(fun _ -> drawPoint())
    window.AddLine.Click.Add(fun _ -> drawLine())
    window.AddVerticalLine.Click.Add(fun _ -> drawVerticalLine())
    window.AddLineSegment.Click.Add(fun _ -> drawLineSegment())
    window.Intersect.Click.Add(fun _ -> intersect())
    window.Root

let com = new System.Uri("/GUI;component/App.xaml", System.UriKind.Relative)
let app = (Application.LoadComponent(com) :?> Application)

[<STAThread>]
app.Run(mainWindow()) |> ignore