(*
    A. Baygeldin (c)  2014
    Intersect
*)

type Geom = 
    | NoPoint
    | Point of float * float
    | Line  of float * float
    | VerticalLine of float
    | LineSegment of (float * float) * (float * float)
    | Intersect of Geom * Geom

let rec getIntersect a =
    let makeLine b =
        match b with
        | LineSegment ((x, y), (x', y')) -> 
            let k = (y' - y) / (x' - x)
            let b = y - k * x
            Line (k, b)
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
        | Point (x, y),  VerticalLine (x') -> if x = x' then a else NoPoint
        | _ -> failwith "Wrong type!"
    let pointLineSegmentIntersect a b =
        match a, b with
        | Point (_),  LineSegment ((x', _), (x'', _)) ->
            let b' = makeLine b
            let a' = pointLineIntersect a b'
            match a' with
            | Point (x, _) ->
                if  (x' < x && x < x'') || (x' > x && x > x'') then 
                    a'           
                else 
                    NoPoint
        | _ -> failwith "Wrong type!"
    let lineVertLineIntersect a b =
        match a, b with
        | Line (x, y),  VerticalLine (x') -> Point (x', x' * x + y)
        | _ -> failwith "Wrong type!"
    let lineLineSegmentIntersect a b =
        match a, b with
        | Line (_),  LineSegment ((x', _), (x'', _)) ->
            let b' = makeLine b
            let a' = lineIntersect a b'
            match a' with
            | Point (x, _) ->
                if  (x' < x && x < x'') || (x' > x && x > x'') then 
                    a'           
                else 
                    NoPoint
            | Line (_) -> b
        | _ -> failwith "Wrong type!"
    let vertLineLineSegmentIntersect a b =
        match a, b with
        | VerticalLine (_), LineSegment((x', _), (x'', _)) ->
            let b' = makeLine b
            let a' = lineVertLineIntersect b' a
            match a' with
            | Point (x, _) -> 
                if  (x' < x && x < x'') || (x' > x && x > x'') then 
                    a'           
                else 
                    NoPoint
        | _ -> failwith "Wrong type!"
    match a with
    | Intersect (a, b) ->
        match a, b with
        | Intersect (_), _ -> getIntersect (Intersect (getIntersect a, b))
        | _ , Intersect (_) -> getIntersect (Intersect (a, getIntersect b))
        | NoPoint, _  | _ , NoPoint -> NoPoint 
        | Point (_), Point (_) -> pointIntersect a b       
        | Point (_), Line (_) -> pointLineIntersect a b
        | Line (_), Point (_) -> pointLineIntersect b a
        | Point (_), VerticalLine (_) -> pointVertLineIntersect a b
        | VerticalLine (_), Point (_) -> pointVertLineIntersect b a
        | Point (_), LineSegment (_) -> pointLineSegmentIntersect a b
        | LineSegment (_), Point (_) -> pointLineSegmentIntersect b a   
        | Line (_), Line (_) -> lineIntersect a b
        | Line (_), VerticalLine (_) -> lineVertLineIntersect a b
        | VerticalLine (_), Line (_) -> lineVertLineIntersect b a
        | Line (_), LineSegment (_) -> lineLineSegmentIntersect a b
        | LineSegment (_), Line (_) -> lineLineSegmentIntersect b a
        | VerticalLine (x), VerticalLine (x') when x = x'-> VerticalLine (x)
        | VerticalLine (x), VerticalLine (x') when x <> x'-> NoPoint
        | VerticalLine (_), LineSegment (_) -> vertLineLineSegmentIntersect a b
        | LineSegment (_), VerticalLine (_) -> vertLineLineSegmentIntersect b a
    | _ -> failwith "Wrong type!"