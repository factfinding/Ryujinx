using ARMeilleure.IntermediateRepresentation;
using System;
using System.Diagnostics;
using System.IO;
using static ARMeilleure.IntermediateRepresentation.Operand;

namespace ARMeilleure.CodeGen.LoongArch64
{
    class Assembler
    {
        public const uint SfFlag = 1u << 31;

        private const int SpRegister = 3;
        private const int ZrRegister = 0;

        private readonly Stream _stream;

        public Assembler(Stream stream)
        {
            _stream = stream;
        }

        public void Add(Operand rd, Operand rj, Operand rk)
        {
            if (rd.Type == OperandType.I64)
            {
                Add_D(rd, rj, rk);
            }
            else
            {
                Add_W(rd, rj, rk);
            }
        }

        public void Add_W(Operand rd, Operand rj, Operand rk)
        {
            WriteInstruction(0x00100000u, rd, rj, rk);
        }

        public void Add_D(Operand rd, Operand rj, Operand rk)
        {
            WriteInstruction(0x00108000u, rd, rj, rk);
        }

        public void Fadd(Operand fd, Operand fj, Operand fk)
        {
            if (fd.Type == OperandType.FP64)
            {
                Fadd_D(fd, fj, fk);
            }
            else
            {
                Fadd_S(fd, fj, fk);
            }
        }

        public void Fadd_S(Operand fd, Operand fj, Operand fk)
        {
            WriteInstruction(0x01008000u, fd, fj, fk);
        }

        public void Fadd_D(Operand fd, Operand fj, Operand fk)
        {
            WriteInstruction(0x01010000u, fd, fj, fk);
        }

        public void WriteInstruction(uint instruction, Operand rj)
        {
            WriteUInt32(instruction | EncodeReg(rj) << 5);
        }

        public void WriteInstruction(uint instruction, Operand rd, Operand rj)
        {
            WriteUInt32(instruction | EncodeReg(rd) | (EncodeReg(rj) << 5));
        }

        public void WriteInstruction(uint instruction, Operand rd, Operand rj, Operand rk)
        {
            WriteUInt32(instruction | EncodeReg(rd) | (EncodeReg(rj) << 5) | (EncodeReg(rk) << 10));
        }

        public void WriteInstruction(uint instruction, Operand rd, Operand rj, Operand rk, Operand ra)
        {
            WriteUInt32(instruction | EncodeReg(rd) | (EncodeReg(rj) << 5) | (EncodeReg(rk) << 10) | (EncodeReg(ra) << 15));
        }

        private static uint EncodeSImm7(int value, int scale)
        {
            uint imm = (uint)(value >> scale) & 0x7f;
            Debug.Assert(((int)imm << 25) >> (25 - scale) == value, $"Failed to encode constant 0x{value:X} with scale {scale}.");
            return imm;
        }

        private static uint EncodeSImm9(int value)
        {
            uint imm = (uint)value & 0x1ff;
            Debug.Assert(((int)imm << 23) >> 23 == value, $"Failed to encode constant 0x{value:X}.");
            return imm;
        }

        private static uint EncodeSImm19_2(int value)
        {
            uint imm = (uint)(value >> 2) & 0x7ffff;
            Debug.Assert(((int)imm << 13) >> 11 == value, $"Failed to encode constant 0x{value:X}.");
            return imm;
        }

        private static uint EncodeSImm26_2(int value)
        {
            uint imm = (uint)(value >> 2) & 0x3ffffff;
            Debug.Assert(((int)imm << 6) >> 4 == value, $"Failed to encode constant 0x{value:X}.");
            return imm;
        }

        private static uint EncodeUImm4(int value)
        {
            uint imm = (uint)value & 0xf;
            Debug.Assert((int)imm == value, $"Failed to encode constant 0x{value:X}.");
            return imm;
        }

        private static uint EncodeUImm6(int value)
        {
            uint imm = (uint)value & 0x3f;
            Debug.Assert((int)imm == value, $"Failed to encode constant 0x{value:X}.");
            return imm;
        }

        private static uint EncodeUImm12(int value, OperandType type)
        {
            return EncodeUImm12(value, GetScaleForType(type));
        }

        private static uint EncodeUImm12(int value, int scale)
        {
            uint imm = (uint)(value >> scale) & 0xfff;
            Debug.Assert((int)imm << scale == value, $"Failed to encode constant 0x{value:X} with scale {scale}.");
            return imm;
        }

        private static uint EncodeUImm16(int value)
        {
            uint imm = (uint)value & 0xffff;
            Debug.Assert((int)imm == value, $"Failed to encode constant 0x{value:X}.");
            return imm;
        }

        private static uint EncodeReg(Operand reg)
        {
            if (reg.Kind == OperandKind.Constant && reg.Value == 0)
            {
                return ZrRegister;
            }

            uint regIndex = (uint)reg.GetRegister().Index;
            Debug.Assert(reg.Kind == OperandKind.Register);
            Debug.Assert(regIndex < 32);
            return regIndex;
        }

        public static int GetScaleForType(OperandType type)
        {
            return type switch
            {
                OperandType.I32 => 2,
                OperandType.I64 => 3,
                OperandType.FP32 => 2,
                OperandType.FP64 => 3,
                OperandType.V128 => 4,
                _ => throw new ArgumentException($"Invalid type {type}."),
            };
        }

#pragma warning disable IDE0051 // Remove unused private member
        private void WriteInt16(short value)
        {
            WriteUInt16((ushort)value);
        }

        private void WriteInt32(int value)
        {
            WriteUInt32((uint)value);
        }

        private void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }
#pragma warning restore IDE0051

        private void WriteUInt16(ushort value)
        {
            _stream.WriteByte((byte)(value >> 0));
            _stream.WriteByte((byte)(value >> 8));
        }

        private void WriteUInt32(uint value)
        {
            _stream.WriteByte((byte)(value >> 0));
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)(value >> 16));
            _stream.WriteByte((byte)(value >> 24));
        }
    }
}
