﻿using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.D3DCompiler;

namespace Core {
    using System;

    using Core.FX;

    using SlimDX.DXGI;
    using SlimDX.Direct3D11;

    using Device = SlimDX.Direct3D11.Device;

    namespace Vertex {


        [StructLayout(LayoutKind.Sequential)]
        public struct VertexPC {
            public Vector3 Pos;
            public Color4 Color;

            public VertexPC(Vector3 pos, Color color) {
                Pos = pos;
                Color = color;
            }

            public VertexPC(Vector3 pos, Color4 color) {
                Pos = pos;
                Color = color;
            }

            public static readonly int Stride = Marshal.SizeOf(typeof (VertexPC));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VertexPN {
            public Vector3 Position;
            public Vector3 Normal;

            public VertexPN(Vector3 position, Vector3 normal) {
                Position = position;
                Normal = normal;
            }

            public static readonly int Stride = Marshal.SizeOf(typeof (VertexPN));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Basic32 {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 Tex;

            public Basic32(Vector3 position, Vector3 normal, Vector2 texC) {
                Position = position;
                Normal = normal;
                Tex = texC;
            }

            public static readonly int Stride = Marshal.SizeOf(typeof (Basic32));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TreePointSprite {
            public Vector3 Pos;
            public Vector2 Size;

            public static readonly int Stride = Marshal.SizeOf(typeof (TreePointSprite));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PosNormalTexTan {
            public Vector3 Pos;
            public Vector3 Normal;
            public Vector2 Tex;
            public Vector3 Tan;
            public static readonly int Stride = Marshal.SizeOf(typeof (PosNormalTexTan));

            public PosNormalTexTan(Vector3 position, Vector3 normal, Vector2 texC, Vector3 tangentU) {
                Pos = position;
                Normal = normal;
                Tex = texC;
                Tan = tangentU;
            }
        }

        public struct TerrainCP {
            public Vector3 Pos;
            public Vector2 Tex;
            public Vector2 BoundsY;

            public TerrainCP(Vector3 pos, Vector2 tex, Vector2 boundsY) {
                Pos = pos;
                Tex = tex;
                BoundsY = boundsY;
            }

            public static readonly int Stride = Marshal.SizeOf(typeof(TerrainCP));
        }
        public struct PosNormalTexTanSkinned {
            public Vector3 Pos;
            public Vector3 Normal;
            public Vector2 Tex;
            public Vector4 Tan;
            public float Weight;
            public BonePalette BoneIndices;

            public static readonly int Stride = Marshal.SizeOf(typeof(PosNormalTexTanSkinned));

            public PosNormalTexTanSkinned(Vector3 pos, Vector3 norm, Vector2 uv, Vector3 tan, float weight, byte[] boneIndices) {
                Pos = pos;
                Normal = norm;
                Tex = uv;
                Tan = new Vector4(tan, 0);
                Weight = weight;
                BoneIndices = new BonePalette();
                for (int index = 0; index < boneIndices.Length; index++) {
                    switch (index) {
                        case 0:
                            BoneIndices.B0 = boneIndices[index];
                            break;
                        case 1:
                            BoneIndices.B1 = boneIndices[index];
                            break;
                        case 2:
                            BoneIndices.B2 = boneIndices[index];
                            break;
                        case 3:
                            BoneIndices.B3 = boneIndices[index];
                            break;
                    }
                }
                
            }
        }
        public struct BonePalette {
            public byte B0, B1, B2, B3;
        }
    }

    public static class InputLayoutDescriptions {
        public static readonly InputElement[] PosColor = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
            new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
        };

        public static readonly InputElement[] Pos = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0 ), 
        };
        public static readonly InputElement[] PosNormal = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
            new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0, InputClassification.PerVertexData, 0)
        };
        public static readonly InputElement[] Basic32 = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
            new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0, InputClassification.PerVertexData, 0), 
            new InputElement("TEXCOORD", 0, Format.R32G32_Float, 24, 0, InputClassification.PerVertexData, 0)
        };
        public static readonly InputElement[] TreePointSprite = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0 ),
            new InputElement("SIZE", 0, Format.R32G32_Float, 12,0,InputClassification.PerVertexData, 0) 
        };
        public static readonly InputElement[] InstancedBasic32 = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
            new InputElement("NORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0), 
            new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElement("WORLD", 0, Format.R32G32B32A32_Float, 0, 1, InputClassification.PerInstanceData, 1 ), 
            new InputElement("WORLD", 1, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1, InputClassification.PerInstanceData, 1 ),
            new InputElement("WORLD", 2, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1, InputClassification.PerInstanceData, 1 ),
            new InputElement("WORLD", 3, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1, InputClassification.PerInstanceData, 1 ),
            new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1, InputClassification.PerInstanceData, 1 )
        };
        public static readonly InputElement[] PosNormalTexTan = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
            new InputElement("NORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0), 
            new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElement("TANGENT", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData,0 ) 
        };

    public static readonly InputElement[] TerrainCP = {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
        new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
        new InputElement("TEXCOORD", 1, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
    };

        public static readonly InputElement[] PosNormalTexTanSkinned = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
            new InputElement("NORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0), 
            new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElement("TANGENT", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData,0 ),
            new InputElement("BLENDWEIGHT", 0, Format.R32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElement("BLENDINDICES", 0, Format.R8G8B8A8_UInt, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0), 
        };
        public static InputElement[] InstancedPosNormalTexTan = {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
            new InputElement("NORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0), 
            new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElement("TANGENT", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData,0 ),
            new InputElement("WORLD", 0, Format.R32G32B32A32_Float, 0, 1, InputClassification.PerInstanceData, 1 ), 
            new InputElement("WORLD", 1, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1, InputClassification.PerInstanceData, 1 ),
            new InputElement("WORLD", 2, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1, InputClassification.PerInstanceData, 1 ),
            new InputElement("WORLD", 3, Format.R32G32B32A32_Float, InputElement.AppendAligned, 1, InputClassification.PerInstanceData, 1 ),
        };  
    }
    public static class InputLayouts {
        public static void InitAll(Device device) {
            var bl1 = Effects.BasicFX;
            if (bl1 != null) {
                try {
                    var passDesc = bl1.Light1Tech.GetPassByIndex(0).Description;
                    if (passDesc.Signature != null) PosNormal = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.PosNormal);
                } catch (Exception dex) {
                    Console.WriteLine(dex.Message );
                    PosNormal = null;
                }
                try {
                    var passDesc = bl1.Light1Tech.GetPassByIndex(0).Description;
                    Basic32 = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.Basic32);
                } catch (Exception dex) {
                    Console.WriteLine(dex.Message );
                    Basic32 = null;
                }
            }
            try {
                var ibl1 = Effects.InstancedBasicFX;
                if (ibl1 != null) {
                    var shaderSignature = ibl1.Light1Tech.GetPassByIndex(0).Description.Signature;
                    InstancedBasic32 = new InputLayout(device, shaderSignature, InputLayoutDescriptions.InstancedBasic32);
                }
            } catch (Exception dex) {
                Console.WriteLine(dex.Message + dex.StackTrace);
                InstancedBasic32 = null;
            }
            try {
                var tsl3 = Effects.TreeSpriteFX;
                if (tsl3 != null) {
                    var passDesc = tsl3.Light3Tech.GetPassByIndex(0).Description;
                    TreePointSprite = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.TreePointSprite);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + ex.StackTrace);
                TreePointSprite = null;
            }
            try {
                var skyTech = Effects.SkyFX;
                if (skyTech != null) {
                    var passDesc = skyTech.SkyTech.GetPassByIndex(0).Description;
                    Pos = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.Pos);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Pos = null;
            }
            try {
                var tech = Effects.NormalMapFX;
                if (tech != null) {
                    var passDesc = tech.Light1Tech.GetPassByIndex(0).Description;
                    PosNormalTexTan = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.PosNormalTexTan);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + ex.StackTrace);
                PosNormalTexTan = null;
            }
            try {
                var tech = Effects.TerrainFX;
                if (tech != null) {
                    var passDesc = tech.Light1Tech.GetPassByIndex(0).Description;
                    TerrainCP = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.TerrainCP);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                TerrainCP = null;
            }
            try {
                var tech = Effects.ColorFX;
                if (tech != null) {
                    var passDesc = tech.ColorTech.GetPassByIndex(0).Description;
                    PosColor = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.PosColor);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                PosColor = null;
            }
            try {
                var tech = Effects.BasicFX;
                if (tech != null) {
                    var passDesc = tech.Light1SkinnedTech.GetPassByIndex(0).Description;
                    PosNormalTexTanSkinned = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.PosNormalTexTanSkinned);
                } else if ((tech = Effects.NormalMapFX) != null) {
                    var passDesc = tech.Light1SkinnedTech.GetPassByIndex(0).Description;
                    PosNormalTexTanSkinned = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.PosNormalTexTanSkinned);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                PosNormalTexTanSkinned = null;
            }
            try {
                var tech = Effects.InstancedNormalMapFX;
                if (tech != null) {
                    var passDesc = tech.Light1Tech.GetPassByIndex(0).Description;
                    InstancedPosNormalTexTan = new InputLayout(device, passDesc.Signature, InputLayoutDescriptions.InstancedPosNormalTexTan);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + ex.StackTrace);
                InstancedPosNormalTexTan = null;
            }
            
        }
        public static void DestroyAll() {
            Util.ReleaseCom(ref Pos);
            Util.ReleaseCom(ref PosNormal);
            Util.ReleaseCom(ref Basic32);
            Util.ReleaseCom(ref TreePointSprite);
            Util.ReleaseCom(ref InstancedBasic32);
            Util.ReleaseCom(ref PosNormalTexTan);
            Util.ReleaseCom(ref TerrainCP);
            Util.ReleaseCom(ref PosColor);
            Util.ReleaseCom(ref PosNormalTexTanSkinned);
            Util.ReleaseCom(ref InstancedPosNormalTexTan);
        }

        public static InputLayout PosNormal;
        public static InputLayout Basic32;
        public static InputLayout TreePointSprite;
        public static InputLayout InstancedBasic32;
        public static InputLayout Pos;
        public static InputLayout PosNormalTexTan;
        public static InputLayout TerrainCP;
        public static InputLayout PosColor;
        public static InputLayout PosNormalTexTanSkinned;
        public static InputLayout InstancedPosNormalTexTan;
    }
}
