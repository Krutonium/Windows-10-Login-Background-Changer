param($p1, $p2, $p3)

$priPath = $p1#$args[0]
$outputPath = $p2#$args[1]
$replacementPath = $p3#$args[2]
$ErrorActionPreference = "Stop"

$inputStream = [System.IO.File]::OpenRead($priPath)
$outputStream = [System.IO.File]::Create($outputPath)
$replacementStream = [System.IO.File]::OpenRead($replacementPath)

$inputReader = New-Object System.IO.BinaryReader -ArgumentList $inputStream
$outputWriter = New-Object System.IO.BinaryWriter -ArgumentList $outputStream
 
$inputStream.CopyTo($outputStream)

$replacementLengthAligned = ([Math]::Ceiling($replacementStream.Length / 8) * 8)

# header
$inputStream.Seek(0x14, "Begin") | Out-Null
$headerLength = $inputReader.ReadUInt32()
$inputStream.Seek(0xB8, "Begin") | Out-Null
$dataitemOffset = $inputReader.ReadUInt32()
$origDataitemLength = $inputReader.ReadUInt32()
$dataitemLength = $origDataitemLength + $replacementLengthAligned
$outputStream.Seek(0xBC, "Begin") | Out-Null
$outputWriter.Write([int]$dataitemLength)

# dataitem
$outputStream.Seek($headerLength + $dataitemOffset + 0x18, "Begin") | Out-Null
$outputWriter.Write([int]$dataitemLength)
$inputStream.Seek($headerLength + $dataitemOffset + 0x24, "Begin") | Out-Null
$stringCount = $inputReader.ReadUInt16()
$blobCount = $inputReader.ReadUInt16()
$origDataLength = $inputReader.ReadUInt32()
$outputStream.Seek(0xC, "Current") | Out-Null
$outputWriter.Write([int]($origDataLength + $replacementLengthAligned))
$outputStream.Seek($stringCount * 4, "Current") | Out-Null
for ($i = 0; $i -lt 10; $i++)
{
    $outputWriter.Write($origDataLength)
    $outputWriter.Write([int]$replacementStream.Length)
}
$outputStream.Seek(($blobCount - 10) * 8, "Current") | Out-Null

# data
$outputStream.Seek($origDataLength, "Current") | Out-Null
if ($outputStream.Length - $outputStream.Position -ne 0x18)
{
    Write-Error "Not compatible with this PRI file."
}
$replacementStream.CopyTo($outputStream)

# footer
$outputStream.Seek($replacementLengthAligned - $replacementStream.Length, "Current") | Out-Null
$outputWriter.Write(0xDEF5FADE)
$outputWriter.Write([int]$dataitemLength)
$outputWriter.Write(0xDEFFFADE)
$outputWriter.Write(0x00000000)
$outputWriter.Write([char[]]"mrm_pri2")

$outputStream.Seek(0xC, "Begin") | Out-Null
$outputWriter.Write([int]$outputStream.Length)
$outputStream.Seek(-0xC, "End") | Out-Null
$outputWriter.Write([int]$outputStream.Length)

$inputReader.Close()
$outputWriter.Close()
$replacementStream.Close()