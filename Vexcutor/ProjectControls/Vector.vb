Public Class Vector

    Protected Overrides Sub OnPaint(ByVal pe As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(pe)

        'Add your custom paint code here
    End Sub
    'define data format
    'base pairs
    ' 5' overhang and 3' overhang and nicks at which part of the side.
    'gdf=general DNA format
    '<Geninfo id=0 name="DNA">
    '<title name=200/><Nick chain=sense antisense/><Overhange id=1 chain=sense><Sequence id=1 value=ACTAGT/></Overhange id=1>
    '<EnzymeSite id=23 enzyme=XbaI></EnzymeSite id=23><Enzyme Site Buffer>
    '<Title id=32>

End Class
