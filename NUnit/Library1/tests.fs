(*
A. Baygeldin (c) 2014
NUnit tests
*)

namespace Tests

open Intersect
open NUnit.Framework

module secondaryFunctions =
    let (==) a b = a = b

[<TestFixture>]
module getIntersectTests =
    open secondaryFunctions
    [<Test>]
    let ``NoPoint with something`` = Assert.AreEqual ((getIntersect (Intersect (NoPoint, Line (13.23423, 4.13239)))) == NoPoint, true)
    [<Test>]
    let ``Intersect of Intersects`` = Assert.AreEqual ((getIntersect (Intersect (Intersect (VerticalLine (36.23423), Point (36.23423, 46.36652)), Intersect (Point (36.23423, 46.36652), LineSegment ((12.42345, 46.36652), (65.234235, 46.36652))))) == Point (36.23423, 46.36652)), true)
    [<Test>]
    let ``LineSegments that don't have intersection on the Y axis`` = Assert.AreEqual ((getIntersect (Intersect (LineSegment ((1.42542, 1.45645), (5.45234, 5.74563)), LineSegment ((1.42542, 6.12153), (6.20965, 18.41353))))) == NoPoint, true)
    [<Test>]
    let ``LineSegment as Line`` = Assert.AreEqual ((getIntersect (Intersect (LineSegment ((4.22864, 7.24682), (1.24682, 9.22864)), Line (-0.6646343508, 10.0573194))) == LineSegment ((4.22864, 7.24682), (1.24682, 9.22864))), true)
    [<Test>]
    let ``LineSegment as VerticalLine`` = Assert.AreEqual ((getIntersect (Intersect (LineSegment ((1.23534, 7.23425), (10.64563, 7.23425)), VerticalLine (7.23425))) == LineSegment ((1.23534, 7.23425), (10.64563, 7.23425))), true)
    [<Test>]
    let ``LineSegment as Point`` = Assert.AreEqual ((getIntersect (Intersect (LineSegment ((3.52345, 7.43457), (3.52345, 7.43457)), Point (3.52345, 7.43457))) == Point (3.52345, 7.43457)), true)
    [<Test>]
    let ``Random LineSegments`` = Assert.AreEqual ((getIntersect (Intersect (LineSegment ((-5.23462, 1.35498), (-10.23696, 13.47954)), LineSegment((-7.23498, 0.23433), (6.20694, 10.98346)))) == Point (-5.383213072, 1.715136569)), true)
    [<Test>]
    let ``Vertical LineSegments have сommon point`` = Assert.AreEqual ((getIntersect (Intersect (LineSegment ((7.25346, 1.23525), (7.25346, 5.92849)), LineSegment ((7.25346, 5.92849), (7.25346, 9.53853)))) == Point (7.25346, 5.92849)), true)
    [<Test>]
    let ``Horizontal LineSegments have сommon point`` = Assert.AreEqual ((getIntersect (Intersect (LineSegment ((1.45236, 7.743701), (4.45236, 10.998268)), LineSegment ((1.45236, 7.743701), (4.45236, 17.507402)))) == Point (1.45236, 7.743701)), true)
    [<Test>]
    let ``Same Points`` = Assert.AreEqual ((getIntersect (Intersect (Point (2.63452, 10.52356), Point (2.63452, 10.52356))) == Point (2.63452, 10.52356)), true)
    [<Test>]
    let ``Parallel Lines`` = Assert.AreEqual ((getIntersect (Intersect (Line (43.23462, 2.23579), Line (43.23462, 9.34643))) == NoPoint), true)
