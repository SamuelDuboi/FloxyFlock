import tkinter as tk

from PIL import ImageTk
import PIL.Image
from tkinter import ttk
from tkinter import *
import json
from pathlib import Path
# initialize tkinter
my_w=tk.Tk()
my_w.geometry('1920x1080')
my_w.title('test')
tabs = ttk.Notebook(my_w);
tabs.pack(fill=BOTH, expand=TRUE)
frame1 = ttk.Frame(tabs)
frame2 = ttk.Frame(tabs)
tabs.add(frame1, text="Tab One")
tabs.add(frame2, text="Tab Two")


#first tab
l1 = tk.Label(frame1,  width=10 )
l1.grid(row=1,column=1) 
ayo_img =ImageTk.PhotoImage(PIL.Image.open("D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\AYO.png").resize((150,150)))
beanie_img =ImageTk.PhotoImage(PIL.Image.open("D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\BEANIE.png").resize((150,150)))
garry_img =ImageTk.PhotoImage(PIL.Image.open("D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\GARRY.png").resize((150,150))) 
hehe_img =ImageTk.PhotoImage(PIL.Image.open("D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\HEHE.png").resize((150,150)))
jcvf_img =ImageTk.PhotoImage(PIL.Image.open("D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\JCVF.png").resize((150,150)))
jit_img = ImageTk.PhotoImage(PIL.Image.open("D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\JIT.png").resize((150,150)))
woo_img = ImageTk.PhotoImage(PIL.Image.open("D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\WOO.png").resize((150,150)))
array_img =[ayo_img,beanie_img,hehe_img,jcvf_img,jit_img,woo_img]
arrayName = ["Ayo : ","Beanie : ","Garry : ","Hehe : ","Jcvf : ","Jit : ","Woo : "]
f=open(Path(r'C:\Users\s.dubois\AppData\LocalLow\FoxyTeam\Foxy Flox\save.json'))
data = json.load(f)
arrayOfFlox = [""]
for i in data['flocks']:
    arrayOfFlox.append(i)
f.close()

for i in range(len(array_img)):
    l2 = tk.Label(frame1,  image=array_img[i] )
    l2.grid(row=i,column=1) 
    l3 = tk.Label(frame1, text =arrayName[i] )
    l3.grid(row= i, column=2)
    l4= tk.Label(frame1, text =arrayOfFlox[i+1] )
    l4.grid(row= i, column=3)



#second tab

canvas=Canvas(frame2, width=1920, height=1080)
canvas.pack()

backgroundImage =ImageTk.PhotoImage(PIL.Image.open("C:\\Users\\s.dubois\\AppData\\LocalLow\\FoxyTeam\\Foxy Flox\\Background.png").resize((900,900)))
dot =ImageTk.PhotoImage(PIL.Image.open("C:\\Users\\s.dubois\\AppData\\LocalLow\\FoxyTeam\\Foxy Flox\\Dot.png").resize((4,4)))

canvas.create_image(450, 450, image=backgroundImage)
canvas.create_image(2, 2, image=dot)
#scrollbar = ttk.Scrollbar(my_w, orient='vertical', command=ayo_img)
#scrollbar.grid(row=0, column=1, sticky='ns')
my_w.mainloop()
# set window title
#root.wm_title("Tkinter window")
#app.
# show window
#root.mainloop()
#C:\Users\s.dubois\AppData\LocalLow\FoxyTeam\Foxy Flox
# Opening JSON file
#f = open('data.json')
 
# returns JSON object as
# a dictionary
#data = json.load(f)
 
# Iterating through the json
# list
#for i in data['emp_details']:
 #   print(i)
 
# Closing file
#f.close()