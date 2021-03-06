﻿//----------------------------------------------------------------------------
//
// Copyright (c) 2014
//
//    Ryan Riley (@panesofglass) and Andrew Cherry (@kolektiv)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//----------------------------------------------------------------------------

[<AutoOpen>]
module internal Freya.Inspector.Prelude

open System.IO
open System.Reflection
open System.Text
open Arachne.Http
open Arachne.Http.Cors
open Arachne.Language
open Chiron
open Freya.Core
open Freya.Machine
open Freya.Machine.Extensions.Http
open Freya.Machine.Extensions.Http.Cors

(* Defaults *)

let defaults =
    freyaMachine {
        using http
        using httpCors

        corsHeadersSupported [ "accept"; "content-type" ]
        corsMethodsSupported [ GET; OPTIONS ]
        corsOriginsSupported AccessControlAllowOriginRange.Any

        charsetsSupported Charset.Utf8
        languagesSupported (LanguageTag.parse "en") }

(* Functions

   Support functions for various aspects of Machine resource
   fulfilment such as reading static resources from embedded assembly
   resources, and negotiating the correct form for representations,
   including JSON serialization when appropriate. *)

(* Resources *)

let private resourceAssembly =
    Assembly.GetExecutingAssembly ()

let resource key =
    use stream = resourceAssembly.GetManifestResourceStream (key)
    use reader = new StreamReader (stream)

    Encoding.UTF8.GetBytes (reader.ReadToEnd ())

(* Representation *)

let private firstNegotiatedOrElse def =
    function | Negotiated (x :: _) -> x
             | _ -> def

let private encode =
    Json.format >> Encoding.UTF8.GetBytes

let represent n x =
    { Description =
        { Charset = Some (n.Charsets |> firstNegotiatedOrElse Charset.Utf8)
          Encodings = None
          MediaType = Some (n.MediaTypes |> firstNegotiatedOrElse MediaType.Text)
          Languages = Some [ n.Languages |> firstNegotiatedOrElse (LanguageTag.parse "en") ] }
      Data = x }

let representJson x =
    { Description =
        { Charset = Some Charset.Utf8
          Encodings = None
          MediaType = Some MediaType.Json
          Languages = Some [ LanguageTag.parse "en" ] }
      Data = encode x }
