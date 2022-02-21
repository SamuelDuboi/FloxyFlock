import tkinter as tk
from tkinter import ttk
# initialize tkinter
my_w=tk.Tk()
my_w.geometry('1500x500')
my_w.title('test')
l1 = tk.Label(my_w,  width=10, height = 1 )
l1.grid(row=1,column=0) 
ayo_img = tk.PhotoImage(file = "D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\AYO.png")
beanie_img = tk.PhotoImage(file = "D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\BEANIE.png")
garry_img = tk.PhotoImage(file = "D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\GARRY.png")
hehe_img = tk.PhotoImage(file = "D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\HEHE.png")
jcvf_img = tk.PhotoImage(file = "D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\JCVF.png")
jit_img = tk.PhotoImage(file = "D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\JIT.png")
woo_img = tk.PhotoImage(file = "D:\\Unity\\FloxyFlock\\FoxyFlockProject\\Assets\\Main\\Art\\Sprites\\Avatars\\WOO.png")
array_img =[ayo_img,beanie_img,hehe_img,jcvf_img,jit_img,woo_img]
arrayName = ["Ayo : ","Beanie : ","Garry : ","Hehe : ","Jcvf : ","Jit : ","Woo : "]
for i in range(len(array_img)):
    l2 = tk.Label(my_w,  image=array_img[i] )
    l2.grid(row=i,column=1) 
    l3 = tk.Label(my_w, text =arrayName[i] )
    l3.grid(row= i, column=2)
#scrollbar = ttk.Scrollbar(my_w, orient='vertical', command=ayo_img)
#scrollbar.grid(row=0, column=1, sticky='ns')
my_w.mainloop()
# set window title
#root.wm_title("Tkinter window")
#app.
# show window
#root.mainloop()
C:\Users\s.dubois\AppData\LocalLow\FoxyTeam\Foxy Flox
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