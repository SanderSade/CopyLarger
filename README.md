# CopyLarger

Copy utility that overwrites existing smaller files with larger files, and copies the non-existing files same as a normal copy

Use: `CopyLarger "source folder" "target folder"`

CopyLarger does not check if the target folder has enough room, nor if the file content differs when the sizes are equal.

Files and folders will be copied with the same structure as source folder has.

Ideas for the future:

* flag to disable all output
* do checking, copying etc on multiple threads
* support input/config file 
