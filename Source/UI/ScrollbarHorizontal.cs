﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WaveTracker.Rendering;

namespace WaveTracker.UI {
    public class ScrollbarHorizontal : Clickable {
        public int totalSize;
        public int viewportSize;
        public Rectangle bar;
        bool lastClickWasOnScrollbar;
        public int ScrollValue { get; set; }
        public int CoarseStepAmount { get; set; }
        int barClickOffset;

        public bool IsVisible { get { return viewportSize < totalSize; } }
        public int barWasPressed;
        public ScrollbarHorizontal(int x, int y, int width, int height, Element parent) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            CoarseStepAmount = 1;
            if (parent != null)
                SetParent(parent);
        }

        public void SetSize(int totalSize, int viewportSize) {
            this.viewportSize = viewportSize;
            this.totalSize = totalSize;
            bar.Height = 6;
            bar.Y = height - bar.Height;
            bar.Width = (int)(width * (viewportSize / (float)totalSize));
        }



        public void Update() {
            if (InFocus) {
                if (enabled) {
                    if (IsVisible) {
                        if (ClickedDown) {
                            lastClickWasOnScrollbar = bar.Contains(LastClickPos);
                            if (MouseY >= bar.Y && MouseY <= bar.Y + bar.Height) {
                                if (lastClickWasOnScrollbar) {
                                    barClickOffset = bar.X - MouseX;
                                }
                                else {
                                    // step bar towards mouse
                                    if (MouseX > bar.X) {
                                        ScrollValue += CoarseStepAmount;
                                    }
                                    else {
                                        ScrollValue -= CoarseStepAmount;
                                    }
                                }
                            }
                        }
                        if (BarisPressed) {
                            bar.X = MouseX + barClickOffset;
                            ScrollValue = (int)Math.Round(BarValFromPos() * (totalSize - viewportSize));
                        }
                        else {
                            if (IsHovered)
                                ScrollValue -= Input.MouseScrollWheel(KeyModifier._Any) * CoarseStepAmount;
                        }
                        UpdateScrollValue();
                    }
                    if (BarisPressed) {
                        barWasPressed = 2;
                    }
                    else {
                        if (barWasPressed > 0)
                            barWasPressed--;
                    }
                }
            }
        }

        public void Draw() {
            if (enabled) {
                if (IsVisible) {

                    Color background = UIColors.panel;
                    Color barSpace = UIColors.labelLight;
                    Color barDefault = ButtonColors.Round.backgroundColor;
                    Color barHover = UIColors.labelDark;
                    Color barPressed = UIColors.black;
                    //DrawRect(0, 0, width, height, new Color(255, 0, 0, 40));

                    DrawRect(0, bar.Y, width, bar.Height, background);
                    DrawRoundedRect(1, bar.Y + 1, width - 2, bar.Height - 2, barSpace);
                    if (BarisPressed && (!Input.internalDialogIsOpen))
                        DrawRoundedRect(bar.X, bar.Y + 1, bar.Width, bar.Height - 2, barPressed);
                    else if (BarisHovered && (!Input.internalDialogIsOpen))
                        DrawRoundedRect(bar.X, bar.Y + 1, bar.Width, bar.Height - 2, barHover);
                    else
                        DrawRoundedRect(bar.X, bar.Y + 1, bar.Width, bar.Height - 2, barDefault);
                }
            }
        }


        /// <summary>
        /// Clamps the scroll value if the scroll value is out of range
        /// </summary>
        public void UpdateScrollValue() {
            if (IsVisible) {
                ScrollValue = Math.Clamp(ScrollValue, 0, totalSize - viewportSize);
                bar.X = (int)Math.Round(BarPosFromVal() * (width - 2) + 1);
            }
        }

        float BarValFromPos() {
            return (bar.X - 1) / (float)(width - 2 - bar.Width);
        }

        float BarPosFromVal() {
            return ScrollValue / (float)totalSize;
        }



        bool BarisHovered => InFocus && bar.Contains(MouseX, MouseY);
        bool BarisPressed => InFocus && Input.GetClick(KeyModifier._Any) && lastClickWasOnScrollbar;
    }
}
