(*
    A. Baygeldin (c) 2014
    Email parser
*)

module Emailregexp

open System.Text.RegularExpressions

//regex101.com - awesome!
let regexp = "^[a-zA-Z]+([\.]?[\w]+)+@([a-zA-Z]+([\-]?[a-zA-Z]+)+\.)+(aero|arpa|asia|biz|cat|com|ru|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|post|pro|tel|travel|xxx)$"
let checkEmail str = Regex.IsMatch(str, regexp)