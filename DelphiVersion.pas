(* These are global flags which should be enabled or disabled depending on the version of Delphi *)
{$if Defined(VER240) or Defined(VER250) or Defined(VER260)}
  {$Define DisableScripting}
  {$Define UseFormatSettings}
{$endif}

(* These are global flags you can enable or disable as you like *)
{$Define Pre15c}
//{$Define RandomizeInVarsHack}
//{$Define GAUSSIAN_DOF}
