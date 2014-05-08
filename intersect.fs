(*
A. Baygeldin (c) 2014
Intersect
*)

module Intersect

type Geom =
    | NoPoint
    | Point of float * float
    | Line of float * float
    | VerticalLine of float
    | LineSegment of (float * float) * (float * float)
    | Intersect of Geom * Geom

let rec getIntersect a =
    let (=) a b =
        let defect = 0.001
        abs(a - b) < defect
    let (>=) a b =
        let defect = 0.001
        a + defect > b
    let (<=) a b =
        let defect = 0.001
        a - defect < b
    let normalizeLineSegment a =
        match a with
        | LineSegment ((x, y), (x', y')) ->
            if ((x' > x) || ((x = x') && (y' > y))) then a else LineSegment ((x', y'), (x, y))
        | _ -> failwith "Wrong type!"
    let parseLineSegment a =
        match a with
        | LineSegment ((x, y), (x', y')) when x <> x' && y <> y' ->
            let k = (y' - y) / (x' - x)
            let b = y - k * x
            Line (k, b)
        | LineSegment ((x, y), (x', y')) when x = x' && y <> y'->
            VerticalLine (x)
        | LineSegment ((x, y), (x', y')) when x = x' && y = y'->
            Point (x, y)
        | _ -> failwith "Wrong type!"
    let lineIntersect a b =
        match a, b with
        | Line (x, y), Line (x', y') ->
            if x = x' then
                if y = y' then a else NoPoint
            else
                let x'' = (y' - y) / (x - x')
                let y'' = x * x'' + y
                Point (x'', y'')
        | _ -> failwith "Wrong type!"
    let pointLineIntersect a b =
        match a, b with
        | Point (x, y), Line (x', y') -> if y = x' * x + y' then a else NoPoint
        | _ -> failwith "Wrong type!"
    let pointIntersect a b =
        match a, b with
        | Point (x, y), Point (x', y') -> if (x = x') && (y = y') then a else NoPoint
        | _ -> failwith "Wrong type!"
    let pointVertLineIntersect a b =
        match a, b with
        | Point (x, y), VerticalLine (x') -> if x = x' then a else NoPoint
        | _ -> failwith "Wrong type!"
    let pointLineSegmentIntersect a b =
        match a, b with
        | Point (_), LineSegment ((x', y'), (x'', y'')) ->
            let b' = parseLineSegment b
            let a' =
                match b' with
                | Line (_) -> pointLineIntersect a b'
                | VerticalLine(_) -> pointVertLineIntersect a b'
                | Point (_) -> pointIntersect a b'
            match a' with
            | Point (x, y) ->
                if (x' <= x && x <= x'') then
                    a'
                else
                    NoPoint
            | NoPoint -> NoPoint
            | _ -> failwith "Wrong type!"
        | _ -> failwith "Wrong type!"
    let lineVertLineIntersect a b =
        match a, b with
        | Line (x, y), VerticalLine (x') -> Point (x', x' * x + y)
        | _ -> failwith "Wrong type!"
    let lineLineSegmentIntersect a b =
        match a, b with
        | Line (_), LineSegment ((x', y'), (x'', y'')) ->
            let b' = parseLineSegment b
            let a' =
                match b' with
                | Line (_) -> lineIntersect a b'
                | VerticalLine(_) -> lineVertLineIntersect a b'
                | Point (_) -> pointLineIntersect b' a
            match a' with
            | Point (x, y) ->
                let buf = (x' <= x)
                let buf2 = (x <= x'')
                if (x' <= x && x <= x'') then
                    a'
                else
                    NoPoint
            | Line (_) -> b
            | NoPoint -> NoPoint
            | _ -> failwith "Wrong type!"
        | _ -> failwith "Wrong type!"
    let vertLineIntersect a b =
        match a, b with
        | VerticalLine (x), VerticalLine (x') when x = x'-> VerticalLine (x)
        | VerticalLine (x), VerticalLine (x') when x <> x'-> NoPoint
        | _ -> failwith "Wrong type!"
    let vertLineLineSegmentIntersect a b =
        match a, b with
        | VerticalLine (_), LineSegment((x', y'), (x'', y'')) ->
            let b' = parseLineSegment b
            let a' =
                match b' with
                | Line (_) -> lineVertLineIntersect b' a
                | VerticalLine(_) -> vertLineIntersect a b'
                | Point (_) -> pointVertLineIntersect b' a
            match a' with
            | Point (x, y) ->
                if (x' <= x && x <= x'') then
                    a'
                else
                    NoPoint
            | VerticalLine (_) -> b
            | NoPoint -> NoPoint
            | _ -> failwith "Wrong type!"
        | _ -> failwith "Wrong type!"
    let lineSegmentIntersect a b =
        match a, b with
        | LineSegment((x0, y0), (x0', y0')), LineSegment((x1, y1), (x1', y1')) ->
            let a' = parseLineSegment a
            let b' = parseLineSegment b
            let a'' = getIntersect (Intersect (a', b'))
            match a'' with
            | Point (x, y) ->
                let my0 = min y0 y0'
                let my0' = max y0 y0'
                let my1 = min y1 y1'
                let my1' = max y1 y1'
                if (x0 <= x && x <= x0') && (x1 <= x && x <= x1') &&
                ((my1 - my0')*(my1' - my0) <= 0.0) then
                    a''
                else
                    NoPoint
            | Line (x, y) -> 
                if ((x1 - x0')*(x1' - x0) > 0.0) then
                    NoPoint
                elif x0' = x1 then
                    Point (x1, y1)
                elif x0 = x1' then
                    Point (x0, y0)
                elif (x0 > x1) && (x1' > x0') then
                    a
                elif (x1 > x0) && (x0' > x1') then
                    b
                elif (x0 > x1) && (x0' > x1') then
                    LineSegment ((x0, y0), (x1', y1'))
                else
                    LineSegment ((x1, y1), (x0', y0'))
            | VerticalLine (x) ->
                if ((y1 - y0')*(y1' - y0) > 0.0) then
                    NoPoint
                elif y0' = y1 then
                    Point (x, y1)
                elif y0 = y1' then
                    Point (x, y0)
                elif (y0 > y1) && (y1' > y0') then
                    a
                elif (y1 > y0) && (y0' > y1') then
                    b
                elif (y0' > y1) && (y0 < y1') then
                    LineSegment ((x, y1), (x, y0'))
                else
                    LineSegment ((x, y0), (x, y1'))
            | NoPoint -> NoPoint
            | _ -> failwith "Wrong type!"
        | _ -> failwith "Wrong type!"
    match a with
    | Intersect (a, b) ->
        match a, b with
        | Intersect (_), _ -> getIntersect (Intersect (getIntersect a, b))
        | _ , Intersect (_) -> getIntersect (Intersect (a, getIntersect b))
        | NoPoint, _ | _ , NoPoint -> NoPoint
        | Point (_), Point (_) -> pointIntersect a b
        | Point (_), Line (_) -> pointLineIntersect a b
        | Line (_), Point (_) -> pointLineIntersect b a
        | Point (_), VerticalLine (_) -> pointVertLineIntersect a b
        | VerticalLine (_), Point (_) -> pointVertLineIntersect b a
        | Point (_), LineSegment (_) -> pointLineSegmentIntersect a (normalizeLineSegment b)
        | LineSegment (_), Point (_) -> pointLineSegmentIntersect b (normalizeLineSegment a)
        | Line (_), Line (_) -> lineIntersect a b
        | Line (_), VerticalLine (_) -> lineVertLineIntersect a b
        | VerticalLine (_), Line (_) -> lineVertLineIntersect b a
        | Line (_), LineSegment (_) -> lineLineSegmentIntersect a (normalizeLineSegment b)
        | LineSegment (_), Line (_) -> lineLineSegmentIntersect b (normalizeLineSegment a)
        | VerticalLine (_), VerticalLine (_) -> vertLineIntersect a b
        | VerticalLine (_), LineSegment (_) -> vertLineLineSegmentIntersect a (normalizeLineSegment b)
        | LineSegment (_), VerticalLine (_) -> vertLineLineSegmentIntersect b (normalizeLineSegment a)
        | LineSegment (_), LineSegment (_) -> lineSegmentIntersect (normalizeLineSegment b) (normalizeLineSegment a)
    | _ -> failwith "Wrong type!"